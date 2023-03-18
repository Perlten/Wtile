
using System.Collections;
using System.Collections.Generic;
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
            _workspaces.Add(new Workspace());
        }
        _currentWorkspace = _workspaces[0];
    }

    private void SetupKeybinds()
    {
        var l = new List<WtileKey> { WtileKey.LWin, WtileKey.D1 };
        //KeybindManager.AddKeybind(new WtileKeybind(l, () => SwitchWorkspace(0)));

        //l = new List<WtileKey> { WtileKey.LWin, WtileKey.D2 };
        //KeybindManager.AddKeybind(new WtileKeybind(l, () => SwitchWorkspace(1)));

        l = new List<WtileKey> { WtileKey.LAlt, WtileKey.D1 };
        KeybindManager.AddKeybind(new WtileKeybind(l, () => Console.WriteLine("test")));


        //l = new List<WtileKey> { WtileKey.Alt, WtileKey.D1 };
        //KeybindManager.AddKeybind(new WtileKeybind(l, () => _currentWorkspace.SwitchWindow(0)));


        //l = new List<WtileKey> { WtileKey.Alt, WtileKey.D2 };
        //KeybindManager.AddKeybind(new WtileKeybind(l, () => _currentWorkspace.SwitchWindow(1)));

    }

    public void Start()
    {
        KeybindManager.StartEventLoop();
        SetupKeybinds();
        _ = WindowHandler.GetNewWindows();
        while (true)
        {
            var newWindows = WindowHandler.GetNewWindows();
            _currentWorkspace.AddWindows(newWindows);
            Console.WriteLine(_currentWorkspace.ToString());

            Thread.Sleep(16);
        }
    }

    private void SwitchWorkspace(int index)
    {
        if (index > _workspaces.Count) return;
        Console.WriteLine(index);
        _currentWorkspace = _workspaces[index];
    }

}
