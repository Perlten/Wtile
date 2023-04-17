using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wtile.Core.Entities;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Core;

public static class Wtile
{
    internal static List<Workspace> _workspaces = new();
    internal static Workspace _currentWorkspace;
    internal static Workspace? _previousWorkspace;

    static Wtile()
    {
        for (int i = 0; i < 10; i++)
        {
            _workspaces.Add(new Workspace(i));
        }
        _currentWorkspace = _workspaces[0];
    }

    public static void Start()
    {
        AppDomain.CurrentDomain.UnhandledException += Logging.WriteUnhandledExceptionToLog;
        KeybindManager.AddToEventLoop();

        while (true)
        {
            KeyMouse.KeyMouse.Update();
            Thread.Sleep(8);
        }
    }

    public static void ChangeWorkspace(int index)
    {
        if (index >= _workspaces.Count) return;
        var workspace = _workspaces[index];
        ChangeWorkspace(workspace);
    }

    public static void ChangeWorkspace(Workspace workspace)
    {
        if (workspace == _currentWorkspace) return;
        _previousWorkspace = _currentWorkspace;
        _currentWorkspace = workspace;
        _currentWorkspace.ChangeToWorkspace();
    }

    internal static Workspace GetCw()
    {
        return _currentWorkspace;
    }

    public static bool AddWindow(IntPtr windowPtr)
    {
        _currentWorkspace.AddWindow(windowPtr);
        return true;
    }

    public static void MoveCurrentWindowToWorkspace(int workspaceIndex)
    {
        if (workspaceIndex >= _workspaces.Count) return;
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        Window? windowToMove = null;
        foreach (var workspace in _workspaces)
        {
            foreach (var window in workspace.Windows)
            {
                if (window.WindowPtr == windowPtr)
                {
                    workspace.RemoveWindow(window);
                    windowToMove = window;
                    break;
                }
            }
        }
        windowToMove ??= new Window(windowPtr, _workspaces[workspaceIndex]);
        _workspaces[workspaceIndex].AddWindow(windowToMove);
    }

    public static void ChangeToPreviousWorkspace()
    {
        if (_previousWorkspace == null) return;
        ChangeWorkspace(_previousWorkspace);
    }

    public static bool RemoveWindow(IntPtr windowPtr)
    {
        foreach (var workspace in _workspaces)
        {
            foreach (var window in workspace.Windows)
            {
                if (window.WindowPtr == windowPtr)
                {
                    workspace.RemoveWindow(window);
                    break;
                }
            }
        }
        return true;
    }

    public static Window GetActiveWindow()
    {
        var windowPtr = ExternalFunctions.GetForegroundWindow();
        Window? window = _workspaces.Select(e => e.Windows.Find(w => w.WindowPtr == windowPtr)).FirstOrDefault();
        if (window == null)
        {
            window = new Window(windowPtr);
            return window;
        }
        return window;
    }

    public static void Quit()
    {
        Application.Exit();
        System.Environment.Exit(0);
    }

    public static string GetWtileString()
    {
        int workspaceIndex = _currentWorkspace.Index + 1;
        int windowIndex = _currentWorkspace.WindowIndex + 1;
        string windowNames = string.Join(" / ", _currentWorkspace.Windows.Select((x, i) => $"{i + 1}: {x.ApplicationName}").ToArray());
        return $"Workspace: {workspaceIndex} | Window: {windowIndex} | {windowNames}";
    }

}
