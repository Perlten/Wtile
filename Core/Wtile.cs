
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Wtile.Core.Config;
using Wtile.Core.Entities;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Core;

public static class Wtile
{
    internal static List<Workspace> _workspaces = new();
    internal static Workspace _currentWorkspace;

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
        //var keys = new List<WtileKey> { WtileKey.LWin };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => { })); // TODO: This is not meant to be


        //keys = new List<WtileKey> { WtileKey.LWin, WtileKey.D1 };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => SwitchWorkspace(0)));

        //keys = new List<WtileKey> { WtileKey.LWin, WtileKey.D2 };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => SwitchWorkspace(1)));

        //keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D1 };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(0)));

        //keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D2 };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(1)));

        //keys = new List<WtileKey> { WtileKey.LAlt, WtileKey.D3 };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.SwitchWindow(2)));

        //keys = new List<WtileKey> { WtileKey.LWin, WtileKey.W };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => _currentWorkspace.AddActiveWindow()));

        //keys = new List<WtileKey> { WtileKey.LWin, WtileKey.LControlKey, WtileKey.LShiftKey, WtileKey.S };
        //KeybindManager.AddKeybind(new WtileKeybind(keys, () => ConfigManager.SaveConfig()));

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
        _currentWorkspace = _workspaces[index];
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
