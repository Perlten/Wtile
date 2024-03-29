﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wtile.Core.Utils;

namespace Wtile.Core.Keybind;

public static class KeybindManager
{

    private static List<WtileKeybind> _keybinds = new();
    private static Dictionary<int, bool> _keymap = new();
    private static HashSet<int> _keysSinceLastGlobalRelease = new();

    const int WM_KEYDOWN = 0x100;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_KEYUP = 0x101;
    const int WM_SYSKEYUP = 0x0105;

    private static bool _keymouseMode = false;
    private static bool _ignoreEvent = false;
    private static int _keySinceLastLwin = 0;


    delegate void EventDelegate();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    static KeybindManager()
    {
    }

    public static void AddToEventLoop()
    {
        EventLoop.EventLoop.AddToEventLoop(new EventDelegate(SetupEvent));
    }

    public static void AddKeybind(WtileKeybind keybind)
    {
        _keybinds.Add(keybind);
        _keybinds.Sort();
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

    private static int HandleKeyMouseEvents()
    {
        if (IsKeyPressed(WtileModKey.LWin) && IsKeyPressed(KeyMouse.KeyMouse.Config.ModKey))
        {
            ReleaseAllKeys();
            SendKeyPress((int)KeyMouse.KeyMouse.Config.ModKey);
            SendKeyRelease((int)KeyMouse.KeyMouse.Config.ModKey);
            return 1;
        }
        _keymouseMode = IsKeyPressed(KeyMouse.KeyMouse.Config.ModKey);
        return _keymouseMode ? 1 : 0;
    }

    private static int HandleKeybindEvent()
    {
        foreach (var keybind in _keybinds)
        {
            if (keybind.ModKeys.All(IsKeyPressed) && IsKeyPressed(keybind.Key))
            {
                keybind.Action();
                return 1;
            }
        }
        return 0;
    }

    private static int HandleLWinKey(int vkCode, IntPtr wParam)
    {
        if (vkCode == (int)WtileModKey.LWin)
        {
            if (IsWparamDown(wParam))
            {
                _keySinceLastLwin = 0;
            }
            else if (IsWparamUp(wParam) && _keySinceLastLwin == 0)
            {
                SendKeyPress((int)WtileModKey.LWin);
                SendKeyRelease((int)WtileModKey.LWin);
            }
            return 1;
        }
        return 0;
    }

    internal static void ReleaseAllKeys()
    {
        foreach (var key in _keymap.Keys)
            SendKeyRelease(key);
    }

    private static IntPtr HandleEvent(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0) return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        int vkCode = Marshal.ReadInt32(lParam);

        if ((int)WtileKey.Pause == vkCode && IsWparamDown(wParam))
        {
            State.RUNNING = !State.RUNNING;
            return 1;
        }

        if (_ignoreEvent || !State.RUNNING)
        {
            _ignoreEvent = false;
            return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        }

        if (IsWparamDown(wParam))
        {
            _keymap[vkCode] = true;
            _keySinceLastLwin++;
            _keysSinceLastGlobalRelease.Add(vkCode);
        }
        else if (IsWparamUp(wParam))
            _keymap.Remove(vkCode);


        if (_keymap.Count == 0 && _keysSinceLastGlobalRelease.Count != 0)
        {
            foreach (var key in _keysSinceLastGlobalRelease)
                SendKeyRelease(key);
            _keysSinceLastGlobalRelease.Clear();
        }

        if (HandleKeyMouseEvents() != 0) return 1;
        if (HandleKeybindEvent() != 0) return 1;
        if (HandleLWinKey(vkCode, wParam) != 0) return 1;

        return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    }


    public static bool IsKeyPressed(WtileKey key)
    {
        return _keymap.ContainsKey((int)key);
    }

    public static bool IsKeyPressed(WtileModKey key)
    {
        return _keymap.ContainsKey((int)key);
    }

    internal static void SendKeyPress(WtileModKey key) { SendKeyPress((int)key); }
    internal static void SendKeyPress(WtileKey key) { SendKeyPress((int)key); }

    internal static void SendKeyPress(int key)
    {
        _ignoreEvent = true;
        ExternalFunctions.keybd_event((byte)key, 0, ExternalFunctions.KEYEVENTF_KEYDOWN, 0);
    }

    internal static void SendKeyRelease(WtileModKey key) { SendKeyRelease((int)key); }
    internal static void SendKeyRelease(WtileKey key) { SendKeyRelease((int)key); }

    internal static void SendKeyRelease(int key)
    {
        _ignoreEvent = true;
        ExternalFunctions.keybd_event((byte)key, 0, ExternalFunctions.KEYEVENTF_KEYUP, 0);
    }
}

