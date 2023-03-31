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
                Gui = GetDefaultGuiConfig(),
                KeyMouse = GetDefaultKeymouse(),
                Keybinds = GetDefaultKeybinds(),
                Rebinds = GetDefaultRebinds()
            };
            return config;
        }

        private static ConfigGui GetDefaultGuiConfig()
        {
            return new ConfigGui()
            {
                Top = 400,
                Left = 400,
                Width = 1500,
                Height = 150,
                Font = new Font("Segoe UI", 14)
            };
        }

        private static List<ConfigKeybinds> GetDefaultRebinds()
        {
            var rebinds = new List<ConfigKeybinds>
            {
                new ConfigKeybinds() { Action = "(", Key = WtileKey.D, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = ")", Key = WtileKey.F, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "/", Key = WtileKey.R, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "\"", Key = WtileKey.J, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "{", Key = WtileKey.K, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "}", Key = WtileKey.L, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "[", Key = WtileKey.C, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "]", Key = WtileKey.V, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "$", Key = WtileKey.G, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "!", Key = WtileKey.Q, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "=", Key = WtileKey.E, ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } },
                new ConfigKeybinds() { Action = "Enter", Key = WtileKey.W, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Escape", Key = WtileKey.Q, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "MediaPlayPause", Key = WtileKey.I, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "MediaNextTrack", Key = WtileKey.O, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "MediaPreviousTrack", Key = WtileKey.U, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Up", Key = WtileKey.K, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Down", Key = WtileKey.J, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Left", Key = WtileKey.H, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Right", Key = WtileKey.L, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "VolumeMute", Key = WtileKey.M, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "VolumeUp", Key = WtileKey.N, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "VolumeDown", Key = WtileKey.B, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Forward", Key = WtileKey.D, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Back", Key = WtileKey.A, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "Home", Key = WtileKey.X, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
                new ConfigKeybinds() { Action = "End", Key = WtileKey.C, ModKeys = { WtileModKey.LAlt, WtileModKey.LControlKey } },
            };

            return rebinds;
        }

        private static List<ConfigKeybinds> GetDefaultKeybinds()
        {
            var keybinds = new List<ConfigKeybinds>();

            var numKeys = new List<WtileKey> { WtileKey.D1, WtileKey.D2, WtileKey.D3, WtileKey.D4, WtileKey.D5, WtileKey.D6, WtileKey.D7, WtileKey.D8, WtileKey.D9 };
            for (var i = 0; i < 9; i++)
            {
                keybinds.Add(new ConfigKeybinds() { Action = $"ChangeWorkspace({i})", Key = numKeys[i], ModKeys = { WtileModKey.LWin } });
                keybinds.Add(new ConfigKeybinds() { Action = $"ChangeWindow({i})", Key = numKeys[i], ModKeys = { WtileModKey.LAlt } });
                keybinds.Add(new ConfigKeybinds() { Action = $"CurrentWindowChangeOrder({i})", Key = numKeys[i], ModKeys = { WtileModKey.LAlt, WtileModKey.LShiftKey } });
                keybinds.Add(new ConfigKeybinds() { Action = $"MoveCurrentWindowToWorkspace({i})", Key = numKeys[i], ModKeys = { WtileModKey.LWin, WtileModKey.LShiftKey } });
            }
            keybinds.Add(new ConfigKeybinds() { Action = "SaveConfig()", Key = WtileKey.S, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey, WtileModKey.LShiftKey } });
            keybinds.Add(new ConfigKeybinds() { Action = "ToggleResizeBar()", Key = WtileKey.H, ModKeys = { WtileModKey.LWin, WtileModKey.LControlKey, WtileModKey.LShiftKey } });
            keybinds.Add(new ConfigKeybinds() { Action = "AddActiveWindow()", Key = WtileKey.W, ModKeys = { WtileModKey.LWin } });
            keybinds.Add(new ConfigKeybinds() { Action = "QuitCurrentWindow()", Key = WtileKey.Q, ModKeys = { WtileModKey.LWin, WtileModKey.LShiftKey } });
            keybinds.Add(new ConfigKeybinds() { Action = "RemoveCurrentWindow()", Key = WtileKey.W, ModKeys = { WtileModKey.LWin, WtileModKey.LShiftKey } });
            keybinds.Add(new ConfigKeybinds() { Action = "ChangeToPreviousWindow()", Key = WtileKey.Q, ModKeys = { WtileModKey.LAlt } });
            keybinds.Add(new ConfigKeybinds() { Action = "ChangeToPreviousWorkspace()", Key = WtileKey.Q, ModKeys = { WtileModKey.LWin } });
            keybinds.Add(new ConfigKeybinds() { Action = "MaximizeWindow()", Key = WtileKey.K, ModKeys = { WtileModKey.LWin } });
            keybinds.Add(new ConfigKeybinds() { Action = "RestoreWindow()", Key = WtileKey.J, ModKeys = { WtileModKey.LWin } });


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
