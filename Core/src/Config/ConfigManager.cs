using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Wtile.Core.Config
{
    public static class ConfigManager
    {
        public static WtileConfig Config { get; private set; }

        static ConfigManager()
        {
            Config = LoadConfig();
        }

        public static void SaveConfig()
        {
            string configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wtile");
            Directory.CreateDirectory(configDir);

            string configPath = Path.Combine(configDir, "config.json");

            string json = JsonConvert.SerializeObject(Config);
            System.IO.File.WriteAllText(configPath, json);
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

    }

    public class WtileConfig
    {
        [JsonPropertyName("testString")]
        public string TestString { get; set; } = "";
    }
}
