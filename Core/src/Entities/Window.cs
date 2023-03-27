﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Wtile.Core.KeyMouse;
using Wtile.Core.Utils;

namespace Wtile.Core.Entities;

public class Window
{
    public readonly IntPtr WindowPtr;
    public readonly string ApplicationName;
    public ExternalFunctions.WindowRect Location { get => GetLocation(); }
    public Screen WindowScreen { get; }
    public string Name { get; }
    public readonly int Id;

    internal Workspace Workspace { get; set; }


    public Window(IntPtr windowPtr, Workspace workspace)
    {
        WindowPtr = windowPtr;
        Name = GetName(WindowPtr);

        WindowScreen = Screen.FromHandle(WindowPtr);
        ExternalFunctions.GetWindowThreadProcessId(WindowPtr, out uint processId);
        Id = (int)processId;
        ApplicationName = Process.GetProcessById(Id).ProcessName.ToString();
        Workspace = workspace;
    }

    private ExternalFunctions.WindowRect GetLocation()
    {
        ExternalFunctions.GetWindowRect(WindowPtr, out ExternalFunctions.WindowRect windowRect);
        return windowRect;
    }

    public void Quit()
    {
        ExternalFunctions.SendMessage(WindowPtr, ExternalFunctions.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        var newWindow = Workspace.Windows.FirstOrDefault();
        if (newWindow != null)
        {
            Workspace.ChangeWindow(newWindow);
        }
        Workspace.SetWindowIndex();
    }

    public void Activate()
    {
        ExternalFunctions.SetForegroundWindow(WindowPtr);
        KeyMouse.KeyMouse.CenterMouseInWindow(this);
    }

    public override string ToString()
    {
        return $"{WindowPtr} -> {Name}";
    }

    internal static string GetName(IntPtr windowPtr)
    {
        int length = ExternalFunctions.GetWindowTextLength(windowPtr);
        var builder = new StringBuilder(length);
        ExternalFunctions.GetWindowText(windowPtr, builder, length + 1);
        var name = builder.ToString();
        return name.Length > 10 ? name[..10] : name;
    }
}
