
using System.Diagnostics;
using Wtile.Core.Config;
using Wtile.Core.Entities;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Core;

public static class Wtile
{
    private static List<Workspace> _workspaces = new();
    private static Workspace _currentWorkspace;

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
        var keys = new List<WtileKey> { WtileKey.LWin, WtileKey.D1 };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => SwitchWorkspace(0)));

        keys = new List<WtileKey> { WtileKey.LWin, WtileKey.D2 };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => SwitchWorkspace(1)));

        keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D1 };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(0)));

        keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D2 };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(1)));

        keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D3 };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(2)));

        keys = new List<WtileKey> { WtileKey.LWin, WtileKey.W };
        KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.AddActiveWindow()));


    }

    public static void Start()
    {
        //KeybindManager.AddToEventLoop();
        //SetupKeybinds();

        ConfigManager.LoadConfig();
        var t = ConfigManager.Config.TestString;
        while (true)
        {
            Debug.WriteLine(_currentWorkspace.ToString());
            Thread.Sleep(16);
        }
    }

    private static void SwitchWorkspace(int index)
    {
        if (index > _workspaces.Count) return;
        Console.WriteLine(index);
        _currentWorkspace = _workspaces[index];
    }

    public static bool AddWindow(IntPtr windowPtr)
    {
        _currentWorkspace.AddWindow(new Window(windowPtr));
        return true;
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
        return $"Workspace: {workspaceIndex} | Window: {windowIndex}";
    }

}
