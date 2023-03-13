namespace Wtile.Core.Entities;
public class Workspace
{
    private List<Window> _windows = new();

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
}
