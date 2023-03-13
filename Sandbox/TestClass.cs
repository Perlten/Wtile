using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


class Program
{
    private static IntPtr _hookHandle;

    //static void Main(string[] args)
    //{
    //    // Set up the hook
    //    var hookProc = new NativeMethods.ShellHookProc(ShellHookCallback);
    //    _hookHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_SHELL, ShellHookCallback, 0, 0);

    //    // Wait for new windows to be created
    //    Console.WriteLine("Waiting for new windows to be created. Press any key to exit.");
    //    Console.ReadKey();

    //    // Unhook the hook
    //    NativeMethods.UnhookWindowsHookEx(_hookHandle);
    //}

    private static IntPtr ShellHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        Console.WriteLine("It worked");
        if (nCode >= 0 && wParam == (IntPtr)NativeMethods.HSHELL_WINDOWCREATED)
        {
            IntPtr hWnd = lParam;
            Console.WriteLine($"New window created with handle {hWnd}");
        }

        return NativeMethods.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
    }
}

static class NativeMethods
{
    public delegate IntPtr ShellHookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public const int HSHELL_WINDOWCREATED = 1;
    public const int WH_SHELL = 10;

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int hookType, ShellHookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
}
