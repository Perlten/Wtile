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

    private volatile static int _keyPressCounter = 0;
    private static WtileModKey? _currentModKey = null;

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
        bool modKeyEvent = Enum.IsDefined(typeof(WtileModKey), vkCode);

        if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
        {
            if (modKeyEvent)
            {
                _currentModKey = (WtileModKey)vkCode;
            }
            else
            {
                if (!_keymap[vkCode]) _keyPressCounter++;
                _keymap[vkCode] = true;
            }
        }
        if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
        {
            if (modKeyEvent)
            {
                _currentModKey = null;
            }
            else
            {
                if (_keymap[vkCode]) _keyPressCounter--;
                _keymap[vkCode] = false;
            }
        }

        if (_currentModKey != null)
        {
            foreach (var keybind in _keybinds)
            {
                if (keybind.ModKey == _currentModKey && keybind.ShouldTrigger(_keymap, _keyPressCounter))
                {
                    keybind.Action();
                    if (keybind.Blocking)
                        return (IntPtr)1; // stops registering the key stroke
                    break;
                }
            }
        }

        if (_currentModKey != null) return 1;
        return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    }
}
