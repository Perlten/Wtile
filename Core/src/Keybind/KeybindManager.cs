using System.CodeDom;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wtile.Core.EventLoop;
using Wtile.Core.Utils;

namespace Wtile.Core.Keybind;

public static class KeybindManager
{
    private static Dictionary<int, bool> _keymap = new();
    private static List<WtileKeybind> _keybinds = new();

    const int WM_KEYDOWN = 0x100;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_KEYUP = 0x101;
    const int WM_SYSKEYUP = 0x0105;

    delegate void EventDelegate();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    static KeybindManager()
    {
        var keyMaxValue = Enum.GetValues(typeof(WtileKey)).Cast<int>().Max() + 1;
        for (int i = 0; i < keyMaxValue; i++) _keymap.Add(i, false);
    }

    public static bool IsKeyPressed(WtileKey key)
    {
        int keyCode = (int)key;
        return _keymap[keyCode];
    }

    public static void AddToEventLoop()
    {
        EventLoop.EventLoop.AddToEventLoop(new EventDelegate(SetupEvent));
    }

    public static void AddKeybind(WtileKeybind keybind)
    {
        _keybinds.Add(keybind);
        _keybinds.Sort();
        _keybinds.Reverse();
    }

    private static void SetupEvent()
    {
        var hHook = ExternalFunctions.SetWindowsHookEx(13, HandleEvent, 0, 0);
        var error = Marshal.GetLastWin32Error();
        if (error != 0)
        {
            string errorMessage = new Win32Exception(error).Message;
            throw new Exception(errorMessage);
        }
    }

    private static IntPtr HandleEvent(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0) return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        int vkCode = Marshal.ReadInt32(lParam);
        //Debug.WriteLine($"Key: {vkCode} --- W: {wParam} --- L:{lParam}");

        if (code >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            _keymap[vkCode] = true;
        }
        if (code >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
        {
            _keymap[vkCode] = false;
        }

        foreach (var keybind in _keybinds)
        {
            if (keybind.HandleTriggering(_keymap))
            {
                if (keybind.Blocking) return (IntPtr)1; // stops registering the key stroke
                break;
            }
        }
        return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    }
}
