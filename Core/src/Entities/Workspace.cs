using Wtile.Core.Config;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;
public class Workspace
{
    internal List<Window> Windows = new();
    internal Window? CurrentWindow { get => GetWindowStackIndex(0); }

    private readonly List<Window> WindowStack = new();

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
        AddWindow(window);
    }
    public void AddWindow(Window window)
    {
        if (ConfigManager.Config.General.IgnoredApplications.Contains(window.ApplicationName))
            return;

        Windows.Add(window);
        AddWindowToStack(window);
        SetWindowIndex();
    }

    public void RemoveWindow(Window window)
    {
        Windows.Remove(window);
        if (window == CurrentWindow)
        {
            RemoveWindowFromStack(window);
            CurrentWindow?.Activate();
        }
        SetWindowIndex();
    }

    public void ChangeToPreviousWindow()
    {
        ChangeWindow(GetWindowStackIndex(1));
    }

    public void ChangeWindow(int index)
    {
        if (Windows.Count - 1 < index) return;
        var window = Windows[index];
        ChangeWindow(window);
    }
    public void ChangeWindow(Window? window)
    {
        if (window == null || window == CurrentWindow) return;
        AddWindowToStack(window);
        CurrentWindow?.Activate();
        SetWindowIndex();
    }

    public void RemoveCurrentWindow()
    {
        if (CurrentWindow == null) return;
        RemoveWindow(CurrentWindow);
        SetWindowIndex();
    }

    public void CurrentWindowChangeOrder(int newIndex)
    {
        var currentWindow = CurrentWindow;
        if (currentWindow == null || newIndex >= Windows.Count) return;
        Windows.Remove(currentWindow);
        Windows.Insert(newIndex, currentWindow);
        SetWindowIndex();
    }

    public void SetWindowIndex()
    {
        var currentWindow = CurrentWindow;
        if (currentWindow == null)
            WindowIndex = 0;
        else
            WindowIndex = Windows.IndexOf(currentWindow);
    }

    public void AddActiveWindow()
    {
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        var window = new Window(windowPtr, this);
        AddWindow(window);
    }

    internal void ChangeToWorkspace()
    {
        if (CurrentWindow == null) return;
        List<Window> selectedWindows = new() { CurrentWindow };
        foreach (var window in WindowStack)
        {
            bool isOverlapping = selectedWindows.Any(e => e.IsOverlapping(window));
            if (!isOverlapping)
                selectedWindows.Add(window);
        }
        selectedWindows.Reverse();
        selectedWindows.ForEach(e => e.Activate(skipMouseCenter: true));
        selectedWindows.Last().CenterMouse();
    }

    private void AddWindowToStack(Window window)
    {
        WindowStack.Remove(window);
        WindowStack.Insert(0, window);
    }

    private void RemoveWindowFromStack(Window window)
    {
        WindowStack.Remove(window);
    }

    private Window? GetWindowStackIndex(int index)
    {
        if (index >= WindowStack.Count) return null;
        return WindowStack[index];
    }
}
