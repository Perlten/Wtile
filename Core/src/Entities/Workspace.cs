﻿using Wtile.Core.Config;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;
public class Workspace
{
    internal List<Window> Windows = new();
    internal Window? CurrentWindow { get; private set; }
    internal Window? PreviousWindow { get; private set; }
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
        PreviousWindow = CurrentWindow;
        CurrentWindow = window;
        SetWindowIndex();
    }

    public void RemoveWindow(Window window)
    {
        Windows.Remove(window);
        if (window == CurrentWindow)
        {
            CurrentWindow = PreviousWindow;
            PreviousWindow = null;
        }
        SetWindowIndex();
    }

    public void ChangeToPreviousWindow()
    {
        if (PreviousWindow == null || CurrentWindow == null) return;
        ChangeWindow(PreviousWindow);
    }

    public void ChangeWindow(int index)
    {
        if (Windows.Count - 1 < index) return;
        var window = Windows[index];
        ChangeWindow(window);
    }
    public void ChangeWindow(Window window)
    {
        if (window == CurrentWindow) return;
        PreviousWindow = CurrentWindow;
        CurrentWindow = window;
        CurrentWindow.Activate();
        SetWindowIndex();
    }

    public void RemoveCurrentWindow()
    {
        if (CurrentWindow == null) return;
        RemoveWindow(CurrentWindow);
        CurrentWindow = Windows.FirstOrDefault(defaultValue: null);
        SetWindowIndex();
    }

    public void CurrentWindowChangeOrder(int newIndex)
    {
        if (CurrentWindow == null || newIndex >= Windows.Count) return;
        Windows.Remove(CurrentWindow);
        Windows.Insert(newIndex, CurrentWindow);
        SetWindowIndex();
    }

    public void SetWindowIndex()
    {
        if (CurrentWindow == null)
            WindowIndex = 0;
        else
            WindowIndex = Windows.IndexOf(CurrentWindow);
    }

    public void AddActiveWindow()
    {
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        var window = new Window(windowPtr, this);
        AddWindow(window);
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
