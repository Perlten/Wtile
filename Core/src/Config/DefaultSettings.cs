using Wtile.Core.Keybind;
using static Wtile.Core.Config.WtileConfig;

namespace Wtile.Core.Config
{
    internal static class DefaultSettings
    {
        internal static WtileConfig GetDefaultConfig()
        {
            var config = new WtileConfig()
            {
                Top = 400,
                Left = 400,
                Width = 1500,
                Height = 150,
                KeyMouse = GetDefaultKeymouse(),
                Keybinds = GetDefaultKeybinds(),
                Rebinds = GetDefaultRebinds()
            };
            return config;
        }

        private static List<ConfigKeybinds> GetDefaultRebinds()
        {
            var rebinds = new List<ConfigKeybinds>();

            var modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            var keybind = new ConfigKeybinds() { Action = "(", Key = WtileKey.D, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = ")", Key = WtileKey.F, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "/", Key = WtileKey.R, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "\"", Key = WtileKey.J, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "{", Key = WtileKey.K, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "}", Key = WtileKey.L, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "[", Key = WtileKey.C, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "]", Key = WtileKey.V, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "$", Key = WtileKey.G, ModKeys = modKeys };
            rebinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LControlKey };
            keybind = new ConfigKeybinds() { Action = "Enter", Key = WtileKey.W, ModKeys = modKeys };
            rebinds.Add(keybind);

            return rebinds;
        }

        private static List<ConfigKeybinds> GetDefaultKeybinds()
        {
            var keybinds = new List<ConfigKeybinds>();
            _ = new List<WtileModKey>();
            _ = new ConfigKeybinds();
            List<WtileModKey>? modKeys;
            ConfigKeybinds? keybind;

            var numKeys = new List<WtileKey> { WtileKey.D1, WtileKey.D2, WtileKey.D3, WtileKey.D4, WtileKey.D5, WtileKey.D6, WtileKey.D7, WtileKey.D8, WtileKey.D9 };
            for (var i = 0; i < 9; i++)
            {
                modKeys = new List<WtileModKey> { WtileModKey.LWin };
                keybind = new ConfigKeybinds() { Action = $"ChangeWorkspace({i})", Key = numKeys[i], ModKeys = modKeys };
                keybinds.Add(keybind);

                modKeys = new List<WtileModKey> { WtileModKey.LAlt };
                keybind = new ConfigKeybinds() { Action = $"ChangeWindow({i})", Key = numKeys[i], ModKeys = modKeys };
                keybinds.Add(keybind);

                modKeys = new List<WtileModKey> { WtileModKey.LAlt, WtileModKey.LShiftKey };
                keybind = new ConfigKeybinds() { Action = $"CurrentWindowChangeOrder({i})", Key = numKeys[i], ModKeys = modKeys };
                keybinds.Add(keybind);

                modKeys = new List<WtileModKey> { WtileModKey.LWin, WtileModKey.LShiftKey };
                keybind = new ConfigKeybinds() { Action = $"MoveCurrentWindowToWorkspace({i})", Key = numKeys[i], ModKeys = modKeys };
                keybinds.Add(keybind);
            }

            modKeys = new List<WtileModKey> { WtileModKey.LWin, WtileModKey.LControlKey, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "SaveConfig()", Key = WtileKey.S, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LWin };
            keybind = new ConfigKeybinds() { Action = "AddActiveWindow()", Key = WtileKey.W, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LWin, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "QuitCurrentWindow()", Key = WtileKey.Q, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LWin, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "RemoveCurrentWindow()", Key = WtileKey.W, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LAlt };
            keybind = new ConfigKeybinds() { Action = "ChangeToPreviousWindow()", Key = WtileKey.Q, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LWin };
            keybind = new ConfigKeybinds() { Action = "ChangeToPreviousWorkspace()", Key = WtileKey.Q, ModKeys = modKeys };
            keybinds.Add(keybind);

            modKeys = new List<WtileModKey> { WtileModKey.LWin, WtileModKey.LControlKey, WtileModKey.LShiftKey };
            keybind = new ConfigKeybinds() { Action = "ToggleResizeBar()", Key = WtileKey.H, ModKeys = modKeys };
            keybinds.Add(keybind);

            return keybinds;
        }

        private static ConfigKeyMouse GetDefaultKeymouse()
        {
            return new ConfigKeyMouse
            {
                Up = WtileKey.W,
                Down = WtileKey.S,
                Left = WtileKey.A,
                Right = WtileKey.D,
                LeftClick = WtileKey.Space,
                RightClick = WtileKey.K,
                SlowDown = WtileKey.J,
                SpeedUp = WtileModKey.LShiftKey,
                ScrollMode = WtileModKey.LAlt,
                ScrollSpeed = 30,
                NormalSpeed = 15,
                SlowSpeed = 5,
                FastSpeed = 25
            };
        }
    }
}
