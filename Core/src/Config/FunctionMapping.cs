using System.Security.AccessControl;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Core.Config;

public static class FunctionMapping
{
    public static readonly Dictionary<string, Action> FunctionMap = new();
    public static readonly Dictionary<string, Action> RebindMap = new();

    static FunctionMapping()
    {
        // Keybinds
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
        fm.Add("ToggleResizeBar()", () => State.RESIZEABLE = !State.RESIZEABLE);

        // Rebinds
        var rm = RebindMap;
        rm.Add("(", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D8));
        rm.Add(")", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D9));
        rm.Add("/", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D7));
        rm.Add("\"", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D2));
        rm.Add("{", CreateAltGrRebind(WtileKey.D7));
        rm.Add("}", CreateAltGrRebind(WtileKey.D0));
        rm.Add("[", CreateAltGrRebind(WtileKey.D8));
        rm.Add("]", CreateAltGrRebind(WtileKey.D9));
        rm.Add("$", CreateAltGrRebind(WtileKey.D4));
        rm.Add("MediaPlayPause", CreateSingleKeyRebind(WtileKey.MediaPlayPause));
        rm.Add("MediaNextTrack", CreateSingleKeyRebind(WtileKey.MediaNextTrack));
        rm.Add("MediaPreviousTrack", CreateSingleKeyRebind(WtileKey.MediaPreviousTrack));
        rm.Add("Enter", CreateSingleKeyRebind(WtileKey.Enter));
    }

    private static Action CreateSingleKeyRebind(WtileKey key)
    {
        return () =>
        {
            KeybindManager.ReleaseAllKeys();
            KeybindManager.SendKeyPress((int)key);
            KeybindManager.SendKeyRelease((int)key);
        };
    }

    private static Action CreateSimpleRebind(WtileModKey modkey, WtileKey key)
    {
        return () =>
        {
            KeybindManager.ReleaseAllKeys();
            KeybindManager.SendKeyPress((int)modkey);
            KeybindManager.SendKeyPress((int)key);
            KeybindManager.SendKeyRelease((int)key);
            KeybindManager.SendKeyRelease((int)modkey);
        };
    }
    private static Action CreateAltGrRebind(WtileKey key)
    {
        return () =>
        {
            KeybindManager.ReleaseAllKeys();
            KeybindManager.SendKeyPress((int)WtileModKey.LAlt);
            KeybindManager.SendKeyPress((int)WtileModKey.LControlKey);
            KeybindManager.SendKeyPress((int)key);
            KeybindManager.SendKeyRelease((int)key);
            KeybindManager.SendKeyRelease((int)WtileModKey.LAlt);
            KeybindManager.SendKeyRelease((int)WtileModKey.LControlKey);
        };
    }
}
