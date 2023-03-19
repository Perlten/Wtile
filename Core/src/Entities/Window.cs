using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

public class Window
{
    public readonly IntPtr WindowPtr;
    public readonly string ApplicationName;
    public string Name { get; }
    public readonly int Id;


    public Window(IntPtr windowPtr)
    {
        WindowPtr = windowPtr;
        Name = GetName(WindowPtr);

        ExternalFunctions.GetWindowThreadProcessId(WindowPtr, out uint processId);
        Id = (int)processId;
        ApplicationName = Process.GetProcessById(Id).ProcessName.ToString();
    }

    public void Quit()
    {
        Process.GetProcessById((int)WindowPtr).Kill();
    }

    public void Activate()
    {
        ExternalFunctions.SetForegroundWindow(WindowPtr);
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
