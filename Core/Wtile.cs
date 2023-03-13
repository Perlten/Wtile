
using System.Collections;
using System.Runtime.InteropServices;
using Wtile.Core.Entities;
using Wtile.Core.Utils;

namespace Wtile.Core;

public class Wtile
{
    private List<Workspace> workspaces = new();
    private Workspace current_workspace;

    public Wtile()
    {
        for (int i = 0; i < 10; i++)
        {
            workspaces.Add(new Workspace());
        }
        current_workspace = workspaces[0];
    }

    public void Start()
    {
        var watch = new System.Diagnostics.Stopwatch();
        while (true)
        {
            watch.Start();

            var newWindows = WindowHandler.GetNewWindows();
            current_workspace.AddWindows(newWindows);

            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            watch.Reset();


            Thread.Sleep(16);
        }
    }
}
