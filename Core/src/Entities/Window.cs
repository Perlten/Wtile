using System.Runtime.InteropServices;
using System.Text;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

public class Window
{
    public readonly IntPtr WindowPtr;
    public string Name { get; }

    public Window(IntPtr windowPtr)
    {
        WindowPtr = windowPtr;
        Name = GetName(WindowPtr);
    }

    public void Activate()
    {
        ExternalFunctions.SetForegroundWindow(WindowPtr);
    }

    public override string ToString()
    {
        return $"{WindowPtr} -> {Name}";
    }

    private static string GetName(IntPtr windowPtr)
    {
        int length = ExternalFunctions.GetWindowTextLength(windowPtr);
        var builder = new StringBuilder(length);
        ExternalFunctions.GetWindowText(windowPtr, builder, length + 1);
        return builder.ToString();
    }
}
