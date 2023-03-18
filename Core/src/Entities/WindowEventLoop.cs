using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wtile.Core.EventLoop;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

internal static class WindowEventLoop
{
    delegate void EventDelegate();
    public static void AddToEventLoop()
    {
        EventLoop.EventLoop.AddToEventLoop(new EventDelegate(SetupEvent));
    }

    private static void SetupEvent()
    {
        ExternalFunctions.RegisterShellHookWindow(EventLoop.EventLoop._wnd.Handle);
        //var hHook = ExternalFunctions.SetWindowsHookEx(10, HandleEvent, IntPtr.Zero, 0);
        var error = Marshal.GetLastWin32Error();
        if (error != 0)
        {
            string errorMessage = new Win32Exception(error).Message;
            throw new Exception(errorMessage);
        }
    }

    //private static IntPtr HandleEvent(int code, IntPtr wParam, IntPtr lParam)
    //{
    //    Debug.WriteLine($"W: {wParam} --- L:{lParam}");

    //    return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
    //}
    private static bool HandleEvent(int t)
    {
        //Debug.WriteLine($"W: {wParam} --- L:{lParam}");

        //return ExternalFunctions.CallNextHookEx(IntPtr.Zero, code, (int)wParam, lParam);
        return false;
    }

}
