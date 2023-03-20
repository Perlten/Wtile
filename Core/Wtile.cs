using Wtile.Core.Entities;
using Wtile.Core.Keybind;

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


    private static void SetupKeybinds()
    {
    }

    public static void Start()
    {
        KeybindManager.AddToEventLoop();
        SetupKeybinds();

        while (true)
        {
            //Debug.WriteLine(KeybindManager._keyPressCounter);
            Thread.Sleep(16);
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
        _previousWorkspace = _currentWorkspace;
        _currentWorkspace = workspace;
        _currentWorkspace.CurrentWindow?.Activate();
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
                    workspace.Windows.Remove(window);
                    break;
                }
            }
        }
        return true;
    }

    public static string GetWtileString()
    {
        int workspaceIndex = _currentWorkspace.Index + 1;
        int windowIndex = _currentWorkspace.WindowIndex + 1;
        string windowNames = string.Join(" / ", _currentWorkspace.Windows.Select((x, i) => $"{i + 1}: {x.ApplicationName}").ToArray());
        return $"Workspace: {workspaceIndex} | Window: {windowIndex} | {windowNames}";
    }

}
