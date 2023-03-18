using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Wtile.Core.Utils;

namespace Wtile.Core.Hotkey;

public static class HotKeyManager
{
    private static volatile MessageWindow _wnd;
    private static volatile IntPtr _hwnd;
    private static ManualResetEvent _windowReadyEvent = new(false);

    private const int WM_HOTKEY = 0x312;



    private static Dictionary<int, Action> actionMap = new();
    private static int threadId;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    static HotKeyManager()
    {
        Thread messageLoop = new(delegate ()
          {
              Application.Run(new MessageWindow());
          });
        threadId = messageLoop.ManagedThreadId;
        messageLoop.Name = "WtileHotkeyMessageLoopThread";
        messageLoop.IsBackground = true;
        messageLoop.Start();
    }

    delegate void TestDelegate();


    public static bool AddHotKey(WtileKey key, WtileKeyModifiers modifiers, Action action)
    {
        //int id = CreateId(key, modifiers);
        //RegisterHotKey(key, modifiers, id);
        //actionMap[id] = action;
        //return true;

        _windowReadyEvent.WaitOne();
        _wnd.Invoke(new TestDelegate(TestFunc));



        return true;
    }

    public static void TestFunc()
    {
        var hHook = ExternalFunctions.SetWindowsHookEx(13, PaintHookProc, 0, 0);
        var error = Marshal.GetLastWin32Error();
        if (error != 0)
        {
            string errorMessage = new Win32Exception(error).Message;
            Console.WriteLine(error);
            Console.WriteLine(errorMessage);
        }
    }

    const int WM_KEYDOWN = 0x100;


    private static IntPtr PaintHookProc(int code, IntPtr wParam, IntPtr lParam)
    {
        //Console.WriteLine($"C: {code} --- W: {wParam} --- L:{lParam}");
        if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (vkCode == (int)VKeys.LWIN)
            {
                Console.WriteLine("You pressed the control key");
                return (IntPtr)1; // stops registering the key stroke
            }

            return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);

        }
        else
            return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    }

    private static int CreateId(WtileKey key, WtileKeyModifiers modifier)
    {
        var id = key.GetHashCode() + modifier.GetHashCode();
        return id;
    }

    private static void RegisterHotKey(WtileKey key, WtileKeyModifiers modifiers, int id)
    {
        _windowReadyEvent.WaitOne();
        _wnd.Invoke(new RegisterHotKeyDelegate(RegisterHotKeyInternal), _hwnd, id, (uint)modifiers, (uint)key);
    }

    public static void RemoveHotkey(int id)
    {
        _wnd.Invoke(new UnRegisterHotKeyDelegate(UnRegisterHotKeyInternal), _hwnd, id);
    }

    delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
    delegate void UnRegisterHotKeyDelegate(IntPtr hwnd, int id);

    private static void RegisterHotKeyInternal(IntPtr hwnd, int id, uint modifiers, uint key)
    {
        ExternalFunctions.RegisterHotKey(hwnd, id, modifiers, key);
    }

    private static void UnRegisterHotKeyInternal(IntPtr hwnd, int id)
    {
        ExternalFunctions.UnregisterHotKey(_hwnd, id);
    }

    private static void OnHotKeyPressed(HotKeyEventArgs e)
    {
        var id = CreateId(e.Key, e.Modifiers);
        var HotKeyPressed = actionMap[id];
        HotKeyPressed();
    }


    private class MessageWindow : Form
    {
        public MessageWindow()
        {
            _wnd = this;
            _hwnd = this.Handle;
            _windowReadyEvent.Set();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                HotKeyEventArgs e = new(m.LParam);
                OnHotKeyPressed(e);
            }

            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            // Ensure the window never becomes visible
            base.SetVisibleCore(false);
        }

    }
}


public class HotKeyEventArgs : EventArgs
{
    public readonly WtileKey Key;
    public readonly WtileKeyModifiers Modifiers;

    public HotKeyEventArgs(WtileKey key, WtileKeyModifiers modifiers, int id)
    {
        Key = key;
        Modifiers = modifiers;
    }

    public HotKeyEventArgs(IntPtr hotKeyParam)
    {
        uint param = (uint)hotKeyParam.ToInt64();
        Key = (WtileKey)((param & 0xffff0000) >> 16);
        Modifiers = (WtileKeyModifiers)(param & 0x0000ffff);
    }
}
