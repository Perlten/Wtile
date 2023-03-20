using Wtile.Core.Utils;

namespace Wtile.Core.Entities;
public class Workspace
{
    internal List<Window> Windows = new();
    internal Window? CurrentWindow { get; private set; }
    public readonly int Index;
    public int WindowIndex { get; private set; }

    public Workspace(int index)
    {
        Index = index;
        WindowIndex = 0;
    }

    public void AddWindow(IntPtr windowPtr)
    {
        var window = new Window(windowPtr, this);
        Windows.Add(window);
        CurrentWindow ??= window;
    }
    public void RemoveWindow(Window window)
    {
        Windows.Remove(window);
    }

    public void ChangeWindow(int index)
    {
        if (Windows.Count - 1 < index) return;
        WindowIndex = index;
        CurrentWindow = Windows[index];
        CurrentWindow.Activate();
    }


    public void AddActiveWindow()
    {
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        var window = new Window(windowPtr, this);
        Windows.Add(window);
        CurrentWindow ??= window;
    }

    public override string ToString()
    {
        string str = string.Empty;
        foreach (Window window in Windows)
        {
            str += window.ToString();
        }
        return str;
    }
}
