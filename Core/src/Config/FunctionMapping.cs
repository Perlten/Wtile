namespace Wtile.Core.Config;

public static class FunctionMapping
{
    public static readonly Dictionary<string, Action> FunctionMap = new();

    static FunctionMapping()
    {
        var fm = FunctionMap;
        for (int i = 0; i < 10; i++)
        {
            int index = i; // Seems dump but needs to be there
            fm.Add($"ChangeWorkspace({index})", () => Wtile.ChangeWorkspace(index));
            fm.Add($"ChangeWindow({index})", () => Wtile.GetCw().ChangeWindow(index));
            fm.Add($"CurrentWindowChangeOrder({index})", () => Wtile.GetCw().CurrentWindowChangeOrder(index));
            fm.Add($"MoveCurrentWindowToWorkspace({index})", () => Wtile.GetCw().MoveCurrentWindowToWorkspace(index));
        }
        fm.Add("SaveConfig()", ConfigManager.SaveConfig);
        fm.Add("AddActiveWindow()", () => Wtile.GetCw().AddActiveWindow());
        fm.Add("QuitCurrentWindow()", () => Wtile.GetCw().CurrentWindow?.Quit());
        fm.Add("RemoveCurrentWindow()", () => Wtile.GetCw().RemoveCurrentWindow());
        fm.Add("ChangeToPreviousWindow()", () => Wtile.GetCw().ChangeToPreviousWindow());
        fm.Add("ChangeToPreviousWorkspace()", () => Wtile.ChangeToPreviousWorkspace());
    }
}
