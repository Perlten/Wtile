namespace Wtile;

using System.Runtime.InteropServices;
using System.Text;

public class Program
{

    public static void Main()
    {

        var wtile = new Core.Wtile();
        wtile.Start();
        //    //Core.Wtile.Start();

        //    //var workspace1 = new Workspace();
        //    //var workspace2 = new Workspace();

        //var watch = new System.Diagnostics.Stopwatch();

        //watch.Start();
        //var windowPointers = GetOpenWindows();
        //watch.Stop();
        //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

        //    //var windowList = new List<Window>();

        //    //foreach (var t in windowPointers)
        //    //{
        //    //    var window = new Window(t.Key);
        //    //    windowList.Add(window);
        //    //    Console.WriteLine(window);
        //    //}

        //    //workspace1.AddWindow(windowList[0]);
        //    //workspace1.AddWindow(windowList[1]);

        //    //workspace2.AddWindow(windowList[2]);
        //    //workspace2.AddWindow(windowList[3]);

        //    //Console.WriteLine("Program Complete");
    }


    public static IDictionary<IntPtr, string> GetOpenWindows()
    {
        IntPtr shellWindow = GetShellWindow();
        Dictionary<IntPtr, string> windows = new();

        EnumWindows(delegate (IntPtr IntPtr, int lParam)
        {
            if (IntPtr == shellWindow) return true;
            if (!IsWindowVisible(IntPtr)) return true;

            int length = GetWindowTextLength(IntPtr);
            if (length == 0) return true;

            StringBuilder builder = new(length);
            GetWindowText(IntPtr, builder, length + 1);

            windows[IntPtr] = builder.ToString();
            return true;

        }, 0);

        return windows;
    }

    public static void ActivateWindow(IntPtr windowId)
    {
        SetForegroundWindow(windowId);
    }

    private delegate bool EnumWindowsProc(IntPtr IntPtr, int lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetActiveWindow(IntPtr IntPtr);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr IntPtr);

    [DllImport("USER32.DLL")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowText(IntPtr IntPtr, StringBuilder lpString, int nMaxCount);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowTextLength(IntPtr IntPtr);

    [DllImport("USER32.DLL")]
    private static extern bool IsWindowVisible(IntPtr IntPtr);

    [DllImport("USER32.DLL")]
    private static extern IntPtr GetShellWindow();

}



