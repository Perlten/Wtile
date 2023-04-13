using Newtonsoft.Json;
using System.Diagnostics;
using Wtile.Core.Keybind;

namespace Wtile.Core.Config
{

    public class WtileConfig
    {
        public ConfigKeyMouse KeyMouse { get; set; } = new();
        public List<ConfigKeybinds> Keybinds { get; set; } = new();
        public List<ConfigKeybinds> Rebinds { get; set; } = new();
        public ConfigGui Gui { get; set; } = new();
        public ConfigGeneral General { get; set; } = new();


        public class ConfigGeneral
        {
            public HashSet<string> IgnoredApplications { get; set; } = new();
        }

        public class ConfigGui
        {
            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public int Left { get; set; } = 0;
            public int Top { get; set; } = 0;
            public Font Font { get; set; } = new Font("Segoe UI", 13);

        }

        public class ConfigKeybinds
        {
            public List<WtileModKey> ModKeys { get; set; } = new();
            public WtileKey Key { get; set; } = new();
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
            public WtileModKey SpeedUp { get; set; } = WtileModKey.LShiftKey;
            public WtileModKey ScrollMode { get; set; } = WtileModKey.LAlt;
            public int ScrollSpeed { get; set; } = 30;

            public int NormalSpeed = 15;
            public int FastSpeed = 25;
            public int SlowSpeed = 5;
        }
    }

    public static class ConfigManager
    {
        public static WtileConfig Config { get; internal set; }

        static ConfigManager()
        {
            Config = LoadConfig();
            SetupKeybindings(Config.Keybinds);
            SetupRebinds(Config.Rebinds);
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
                    return HandleUnsetKeybindsAndRebinds(config);

            }
            return DefaultSettings.GetDefaultConfig();
        }
        private static void SetupRebinds(List<WtileConfig.ConfigKeybinds> Keybinds)
        {
            var rebindMap = FunctionMapping.RebindMap;
            foreach (var rebindConfig in Keybinds)
            {
                var action = rebindMap[rebindConfig.Action];
                var keybind = new WtileKeybind(rebindConfig.Key, rebindConfig.ModKeys, rebindMap[rebindConfig.Action]);
                KeybindManager.AddKeybind(keybind);
            }
        }

        private static void SetupKeybindings(List<WtileConfig.ConfigKeybinds> Keybinds)
        {
            var actionMap = FunctionMapping.FunctionMap;
            foreach (var Configkeybind in Keybinds)
            {
                var action = actionMap[Configkeybind.Action];
                var keybind = new WtileKeybind(Configkeybind.Key, Configkeybind.ModKeys, actionMap[Configkeybind.Action]);
                KeybindManager.AddKeybind(keybind);
            }
        }

        private static WtileConfig HandleUnsetKeybindsAndRebinds(WtileConfig config)
        {
            var defaultConfig = DefaultSettings.GetDefaultConfig();
            var keybindsActions = config.Keybinds.Select(e => e.Action).ToHashSet();

            var missingKeybinds = defaultConfig.Keybinds.Where(k => !keybindsActions.Contains(k.Action));
            config.Keybinds.AddRange(missingKeybinds);

            var rebindsActions = config.Rebinds.Select(e => e.Action).ToHashSet();
            var missingRebinds = defaultConfig.Rebinds.Where(r => !rebindsActions.Contains(r.Action));
            config.Rebinds.AddRange(missingRebinds);

            var missingIgnoredApplications = defaultConfig.General.IgnoredApplications.Where(e => !config.General.IgnoredApplications.Contains(e));
            config.General.IgnoredApplications.UnionWith(missingIgnoredApplications);

            return config;
        }

    }

}
