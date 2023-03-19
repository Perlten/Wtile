using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Wtile.Core.Keybind;

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
        public List<ConfigKeybinds> Keybinds { get; set; } = new();


        public class ConfigKeybinds
        {
            public List<WtileKey> Keys { get; set; } = new();
            public string Action { get; set; } = "";
        }

    }

    public static class ConfigManager
    {
        public static WtileConfig Config { get; private set; }

        static ConfigManager()
        {
            Config = LoadConfig();
            SetupKeybindings(Config.Keybinds);
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
                var keybind = new WtileKeybind(Configkeybind.Keys, actionMap[Configkeybind.Action]);
                KeybindManager.AddKeybind(keybind);
            }

        }

    }

}
