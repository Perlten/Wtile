
using System.Collections;
using System.Runtime.InteropServices;
using Wtile.Core.Entities;
using Wtile.Core.Hotkey;
using Wtile.Core.Utils;

namespace Wtile.Core;

public class Wtile
{
    private List<Workspace> workspaces = new();
    private Workspace currentWorkspace;

    public Wtile()
    {
        for (int i = 0; i < 10; i++)
        {
            workspaces.Add(new Workspace());
        }
        currentWorkspace = workspaces[0];
    }

    public void Start()
    {
        SetupKeybinds();
        _ = WindowHandler.GetNewWindows();
        while (true)
        {
            var newWindows = WindowHandler.GetNewWindows();
            currentWorkspace.AddWindows(newWindows);
            Console.WriteLine(currentWorkspace.ToString());

            Thread.Sleep(16);
        }
    }

    private void SetupKeybinds()
    {
        //HotKeyManager.AddHotKey(WtileKey.A, WtileKeyModifiers.Windows,
        //    () => SwitchWorkspace(0));
        //HotKeyManager.AddHotKey(WtileKey.B, WtileKeyModifiers.Windows,
        //    () => SwitchWorkspace(1));

        // Windows
        //HotKeyManager.AddHotKey(WtileKey.A, WtileKeyModifiers.Alt,
        //    () => currentWorkspace.SwitchWindow(0));
        //HotKeyManager.AddHotKey(WtileKey.B, WtileKeyModifiers.Alt,
        //    () => currentWorkspace.SwitchWindow(1));
    }

    private void SwitchWorkspace(int index)
    {
        Console.WriteLine(index);
        if (index < workspaces.Count) return;
        currentWorkspace = workspaces[index];
    }

}
