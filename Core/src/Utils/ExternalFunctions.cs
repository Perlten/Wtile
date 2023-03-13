using System.Runtime.InteropServices;
using System.Text;

namespace Wtile.Core.Utils
{
    internal class ExternalFunctions
    {
        public delegate bool EnumWindowsProc(IntPtr IntPtr, int lParam);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr IntPtr);

        [DllImport("USER32.DLL")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        public static extern int GetWindowText(IntPtr IntPtr, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        public static extern int GetWindowTextLength(IntPtr IntPtr);

        [DllImport("USER32.DLL")]
        public static extern bool IsWindowVisible(IntPtr IntPtr);

        [DllImport("USER32.DLL")]
        public static extern IntPtr GetShellWindow();

    }
}
