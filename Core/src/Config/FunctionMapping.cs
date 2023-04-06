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
        fm.Add("RemoveCurrentWindow()", () => Wtile.GetCw().RemoveCurrentWindow());
        fm.Add("ChangeToPreviousWindow()", () => Wtile.GetCw().ChangeToPreviousWindow());
        fm.Add("ChangeToPreviousWorkspace()", () => Wtile.ChangeToPreviousWorkspace());
        fm.Add("ToggleResizeBar()", () => State.RESIZEABLE = !State.RESIZEABLE);
        fm.Add("QuitCurrentWindow()", () => Wtile.GetActiveWindow().Quit());
        fm.Add("MaximizeWindow()", () => Wtile.GetActiveWindow().Maximize());
        fm.Add("RestoreWindow()", () => Wtile.GetActiveWindow().Restore());
        fm.Add("Quit()", () => Wtile.Quit());

        // Rebinds
        var rm = RebindMap;
        rm.Add("(", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D8));
        rm.Add(")", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D9));
        rm.Add("/", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D7));
        rm.Add("\"", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D2));
        rm.Add("!", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D1));
        rm.Add("=", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.D0));
        rm.Add("Back", CreateSimpleRebind(WtileModKey.LAlt, WtileKey.Left));
        rm.Add("Forward", CreateSimpleRebind(WtileModKey.LAlt, WtileKey.Right));
        rm.Add("MoveWindowLeft", CreateSimpleRebind(WtileModKey.LWin, WtileKey.Left));
        rm.Add("MoveWindowRight", CreateSimpleRebind(WtileModKey.LWin, WtileKey.Right));
        rm.Add("_", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.Dash));
        rm.Add(":", CreateSimpleRebind(WtileModKey.LShiftKey, WtileKey.Dot));

        rm.Add("{", CreateAltGrRebind(WtileKey.D7));
        rm.Add("}", CreateAltGrRebind(WtileKey.D0));
        rm.Add("[", CreateAltGrRebind(WtileKey.D8));
        rm.Add("]", CreateAltGrRebind(WtileKey.D9));
        rm.Add("$", CreateAltGrRebind(WtileKey.D4));

        rm.Add("MediaPlayPause", CreateSingleKeyRebind(WtileKey.MediaPlayPause));
        rm.Add("MediaNextTrack", CreateSingleKeyRebind(WtileKey.MediaNextTrack));
        rm.Add("MediaPreviousTrack", CreateSingleKeyRebind(WtileKey.MediaPreviousTrack));
        rm.Add("Enter", CreateSingleKeyRebind(WtileKey.Enter));
        rm.Add("Left", CreateSingleKeyRebind(WtileKey.Left));
        rm.Add("Right", CreateSingleKeyRebind(WtileKey.Right));
        rm.Add("Up", CreateSingleKeyRebind(WtileKey.Up));
        rm.Add("Down", CreateSingleKeyRebind(WtileKey.Down));
        rm.Add("VolumeMute", CreateSingleKeyRebind(WtileKey.VolumeMute));
        rm.Add("VolumeDown", CreateSingleKeyRebind(WtileKey.VolumeDown));
        rm.Add("VolumeUp", CreateSingleKeyRebind(WtileKey.VolumeUp));
        rm.Add("Escape", CreateSingleKeyRebind(WtileKey.Escape));
        rm.Add("Home", CreateSingleKeyRebind(WtileKey.Home));
        rm.Add("End", CreateSingleKeyRebind(WtileKey.End));
        rm.Add("Backspace", CreateSingleKeyRebind(WtileKey.Backspace));
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
