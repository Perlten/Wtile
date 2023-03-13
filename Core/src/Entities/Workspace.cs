using System.Transactions;

namespace Wtile.Core.Entities;
public class Workspace
{
    private List<Window> _windows = new();
    private Window? currentWindow;

    public Workspace()
    {

    }

    public void AddWindow(Window window)
    {
        _windows.Add(window);
    }
    public void AddWindows(IList<Window> windows)
    {
        _windows.AddRange(windows);
    }


    public void SwitchWindow(int index)
    {
        if (_windows.Count - 1 < index) return;

        currentWindow = _windows[index];
        currentWindow.Activate();
    }

    public void ActivateCurrentWindow()
    {
        if (currentWindow == null) return;
        currentWindow.Activate();
    }

    public override string ToString()
    {
        string str = string.Empty;
        foreach (Window window in _windows)
        {
            str += window.ToString();
        }
        return str;
    }
}
