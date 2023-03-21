﻿using Newtonsoft.Json;
using System.Diagnostics;
using Wtile.Core.Keybind;
using Wtile.Core.KeyMouse;

namespace Wtile.Core.Config
{

    public class WtileConfig
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int Left { get; set; } = 0;
        public int Top { get; set; } = 0;
        public ConfigKeyMouse KeyMouse { get; set; } = new();
        public List<ConfigKeybinds> Keybinds { get; set; } = new();


        public class ConfigKeybinds
        {
            public WtileModKey ModKey { get; set; }
            public List<WtileKey> Keys { get; set; } = new();
            public string Action { get; set; } = "";
        }

        public class ConfigKeyMouse
        {
            public WtileModKey ModKey { get; set; } = WtileModKey.AE;

            public WtileKey Up { get; set; } = WtileKey.W;
            public WtileKey Down { get; set; } = WtileKey.S;
            public WtileKey Left { get; set; } = WtileKey.A;
            public WtileKey Right { get; set; } = WtileKey.D;
            public WtileKey LeftClick { get; set; } = WtileKey.Space;
            public WtileKey RightClick { get; set; } = WtileKey.K;
            public WtileKey SlowDown { get; set; } = WtileKey.J;
            public WtileKey SpeedUp { get; set; } = WtileKey.LShiftKey;
            public WtileModKey ScrollMode { get; set; } = WtileModKey.LAlt;
            public int ScrollSpeed { get; set; } = 30;

            public int NormalSpeed = 15;
            public int FastSpeed = 25;
            public int SlowSpeed = 5;
        }

    }

    public static class ConfigManager
    {
        public static WtileConfig Config { get; private set; }

        static ConfigManager()
        {
            Config = LoadConfig();
            SetupKeybindings(Config.Keybinds);
            KeyMouse.KeyMouse.Config = Config.KeyMouse;
        }

        public static void SaveConfig()
        {
            string configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wtile");
            Directory.CreateDirectory(configDir);

            string configPath = Path.Combine(configDir, "config.json");

            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(configPath, json);
            Debug.WriteLine("Saved config");
        }
        public static WtileConfig LoadConfig()
        {
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wtile", "config.json");

            if (File.Exists(configFilePath))
            {
                string json = File.ReadAllText(configFilePath);
                WtileConfig? config = JsonConvert.DeserializeObject<WtileConfig>(json);
                if (config != null)
                    return config;
                else
                    return new WtileConfig();
            }
            else
                return new WtileConfig();
        }


        private static void SetupKeybindings(List<WtileConfig.ConfigKeybinds> Keybinds)
        {
            var actionMap = FunctionMapping.FunctionMap;
            foreach (var Configkeybind in Keybinds)
            {
                var action = actionMap[Configkeybind.Action];
                var keybind = new WtileKeybind(Configkeybind.Keys, Configkeybind.ModKey, actionMap[Configkeybind.Action]);
                KeybindManager.AddKeybind(keybind);
            }
        }

    }

}
