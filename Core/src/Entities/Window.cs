using System.Runtime.InteropServices;
using System.Text;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

public class Window
{
    private readonly IntPtr _windowPtr;
    public string Name { get; }

    public Window(IntPtr windowPtr)
    {
        _windowPtr = windowPtr;
        Name = GetName(_windowPtr);
    }

    public void Activate()
    {
        ExternalFunctions.SetForegroundWindow(_windowPtr);
    }

    public override string ToString()
    {
        return $"{_windowPtr} -> {Name}";
    }

    private static string GetName(IntPtr windowPtr)
    {
        int length = ExternalFunctions.GetWindowTextLength(windowPtr);
        var builder = new StringBuilder(length);
        ExternalFunctions.GetWindowText(windowPtr, builder, length + 1);
        return builder.ToString();
    }
}
