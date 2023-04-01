using System.Diagnostics;
using System.Text;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

public class Window
{
    public readonly IntPtr WindowPtr;
    public readonly string ApplicationName;
    public ExternalFunctions.WindowRect Location { get => GetLocation(); }
    public Screen WindowScreen { get; }
    public string Name { get; }
    public readonly int Id;

    internal Workspace? Workspace { get; set; }

    public Window(IntPtr windowPtr) : this(windowPtr, null) { }


    public Window(IntPtr windowPtr, Workspace? workspace)
    {
        WindowPtr = windowPtr;
        Name = GetName(WindowPtr);

        WindowScreen = Screen.FromHandle(WindowPtr);
        ExternalFunctions.GetWindowThreadProcessId(WindowPtr, out uint processId);
        Id = (int)processId;
        ApplicationName = Process.GetProcessById(Id).ProcessName.ToString();
        Workspace = workspace;
    }

    private ExternalFunctions.WindowRect GetLocation()
    {
        ExternalFunctions.GetWindowRect(WindowPtr, out ExternalFunctions.WindowRect windowRect);
        return windowRect;
    }

    public void Quit()
    {
        ExternalFunctions.SendMessage(WindowPtr, ExternalFunctions.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        Wtile.RemoveWindow(WindowPtr);

        if (Workspace == null) return;

        var newWindow = Workspace.Windows.FirstOrDefault();
        if (newWindow != null)
        {
            Workspace.ChangeWindow(newWindow);
        }
        Workspace.SetWindowIndex();
    }

    public void Activate()
    {
        // Get the thread ID of the current thread and the thread ID of the window's thread
        int currentThreadId = ExternalFunctions.GetCurrentThreadId();
        int windowThreadId = ExternalFunctions.GetWindowThreadProcessId(WindowPtr, IntPtr.Zero);

        // Attach the input processing mechanism of the current thread to the window's thread
        bool attached = ExternalFunctions.AttachThreadInput(windowThreadId, currentThreadId, true);
        if (attached)
        {
            for (int i = 0; i < 20; i++)
            {
                // Activate the window
                var sucessfullActivated = ExternalFunctions.SetForegroundWindow(WindowPtr);

                if (sucessfullActivated)
                    break;
                Thread.Sleep(10);
            }
            // Detach the input processing mechanism of the two threads
            ExternalFunctions.AttachThreadInput(windowThreadId, currentThreadId, false);

            // Center the mouse in the window
            KeyMouse.KeyMouse.CenterMouseInWindow(this);
        }
    }

    internal void Maximize()
    {
        ExternalFunctions.ShowWindow(WindowPtr, ExternalFunctions.SW_MAXIMIZE);
    }
    internal void Restore()
    {
        ExternalFunctions.ShowWindow(WindowPtr, ExternalFunctions.SW_RESTORE);
    }

    public override string ToString()
    {
        return $"{WindowPtr} -> {Name}";
    }

    internal static string GetName(IntPtr windowPtr)
    {
        int length = ExternalFunctions.GetWindowTextLength(windowPtr);
        var builder = new StringBuilder(length);
        ExternalFunctions.GetWindowText(windowPtr, builder, length + 1);
        var name = builder.ToString();
        return name.Length > 10 ? name[..10] : name;
    }
}
