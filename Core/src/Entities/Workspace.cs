using System.Diagnostics;
using System.Transactions;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;
public class Workspace
{
    internal List<Window> Windows = new();
    private Window? currentWindow;
    public readonly int Index;
    public int WindowIndex { get; private set; }

    public Workspace(int index)
    {
        Index = index;
        WindowIndex = 0;
    }

    public void AddWindow(Window window)
    {
        Windows.Add(window);
    }
    public void AddWindows(IList<Window> windows)
    {
        Windows.AddRange(windows);
    }


    public void SwitchWindow(int index)
    {
        if (Windows.Count - 1 < index) return;
        WindowIndex = index;
        currentWindow = Windows[index];
        currentWindow.Activate();
    }

    public void ActivateCurrentWindow()
    {
        if (currentWindow == null) return;
        currentWindow.Activate();
    }

    public void AddActiveWindow()
    {
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        var window = new Window(windowPtr);
        Windows.Add(window);
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
