
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wtile.Core.Entities;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Core;

public class Wtile
{
    private List<Workspace> _workspaces = new();
    private Workspace _currentWorkspace;

    public Wtile()
    {
        for (int i = 0; i < 10; i++)
        {
            _workspaces.Add(new Workspace(i));
        }
        _currentWorkspace = _workspaces[0];
    }

    private void SetupKeybinds()
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

    }

    public void Start()
    {
        KeybindManager.AddToEventLoop();
        SetupKeybinds();
        _ = WindowHandler.GetNewWindows();
        while (true)
        {
            //var newWindows = WindowHandler.GetNewWindows();
            //_currentWorkspace.AddWindows(newWindows);
            Debug.WriteLine(_currentWorkspace.ToString());

            Thread.Sleep(16);
        }
    }

    private void SwitchWorkspace(int index)
    {
        if (index > _workspaces.Count) return;
        Console.WriteLine(index);
        _currentWorkspace = _workspaces[index];
    }

    public bool AddWindow(IntPtr windowPtr)
    {
        _currentWorkspace.AddWindow(new Window(windowPtr));
        return true;
    }

    public bool RemoveWindow(IntPtr windowPtr)
    {

        return true;
    }

    public string GetWtileString()
    {
        int workspaceIndex = _currentWorkspace.Index;
        int windowIndex = _currentWorkspace.WindowIndex;
        return $"Workspace: {workspaceIndex} | Window: {windowIndex}";
    }

}
