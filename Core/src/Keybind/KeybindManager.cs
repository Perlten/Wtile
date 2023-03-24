using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wtile.Core.Utils;

namespace Wtile.Core.Keybind;

public static class KeybindManager
{
    public class IgnoreKey
    {
        public bool Pressed { get; set; } = false;
        public bool Relased { get; set; } = false;
        public readonly int KeyCode;
        public IgnoreKey(int keyCode)
        {
            KeyCode = keyCode;
        }

        public override string? ToString()
        {
            return $"P: {Pressed} -- R: {Relased} -- K: {KeyCode}";
        }
    }

    private static Dictionary<int, bool> _keymap = new();
    private static List<WtileKeybind> _keybinds = new();

    const int WM_KEYDOWN = 0x100;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_KEYUP = 0x101;
    const int WM_SYSKEYUP = 0x0105;

    private volatile static int _keyPressCounter = 0;
    private static WtileModKey? _currentModKey = null;
    private static int _keysSinceModPress = 0;

    internal static bool _ignoreEvents = false; // Indicates if Wtile should ignore events

    private static bool _keymouseMode = false;

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

    public static bool IsKeyPressed(WtileModKey key)
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

    private static bool IsWparamDown(IntPtr wParam)
    {
        return wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN;
    }
    private static bool IsWparamUp(IntPtr wParam)
    {
        return wParam == WM_KEYUP || wParam == WM_SYSKEYUP;
    }

    private static int HandleKeyMouseEvents(int vkCode, bool modKeyEvent, IntPtr wParam)
    {
        if (_keymouseMode)
        {
            if (IsWparamUp(wParam))
            {
                if (modKeyEvent)
                {
                    var mk = (WtileModKey)vkCode;
                    if (mk == KeyMouse.KeyMouse.Config.ModKey)
                    {
                        _keymouseMode = false;
                    }
                }
            }
            return 1;
        }

        if (modKeyEvent)
        {
            if (IsWparamDown(wParam))
            {
                var mk = (WtileModKey)vkCode;
                if (mk == KeyMouse.KeyMouse.Config.ModKey)
                {
                    _keymouseMode = true;
                    return 1;
                }
            }
        }
        return 0;
    }

    private static int HandleKeybindEvent(int vkCode, bool modKeyEvent, IntPtr wParam)
    {
        if (IsWparamDown(wParam))
        {
            if (modKeyEvent)
            {
                _keysSinceModPress = 0;
                _currentModKey = (WtileModKey)vkCode;
                return 1;
            }
            else
                _keysSinceModPress++;
        }
        if (IsWparamUp(wParam))
        {
            if (modKeyEvent)
            {
                if (_keysSinceModPress == 0 && _currentModKey != null) // If a mod key is pressed alone just pres that mod key
                {
                    SendKeyPress((int)_currentModKey);
                    SendKeyRelease((int)_currentModKey);
                }
                _currentModKey = null;
                return 1;
            }
        }

        if (_currentModKey != null)
        {
            WtileKey key = (WtileKey)vkCode;
            foreach (var keybind in _keybinds)
            {
                if (keybind.ModKey == _currentModKey && keybind.ShouldTrigger(_keymap, _keyPressCounter))
                {
                    keybind.Action();
                    if (keybind.Blocking)
                        return 1; // stops registering the key stroke
                    else
                        return 0;
                }
            }
        }

        return 0;
    }


    private static IntPtr HandleEvent(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0) return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        if (_ignoreEvents || !State.RUNNING)
        {
            _ignoreEvents = false;
            return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        }

        int vkCode = Marshal.ReadInt32(lParam);
        bool modKeyEvent = Enum.IsDefined(typeof(WtileModKey), vkCode);

        if (IsWparamDown(wParam))
        {
            if (!_keymap[vkCode]) _keyPressCounter++;
            _keymap[vkCode] = true;
        }
        else if (IsWparamUp(wParam))
        {
            if (_keymap[vkCode]) _keyPressCounter--;
            _keymap[vkCode] = false;
        }

        if (HandleKeyMouseEvents(vkCode, modKeyEvent, wParam) != 0) return 1;
        if (HandleKeybindEvent(vkCode, modKeyEvent, wParam) != 0) return 1;

        return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    }

    internal static void SendKeyPress(int key)
    {
        _ignoreEvents = true;
        ExternalFunctions.keybd_event((byte)key, 0, ExternalFunctions.KEYEVENTF_KEYDOWN, 0);
    }
    internal static void SendKeyRelease(int key)
    {
        _ignoreEvents = true;
        ExternalFunctions.keybd_event((byte)key, 0, ExternalFunctions.KEYEVENTF_KEYUP, 0);
    }

}
