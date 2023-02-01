using System.Text.Json.Serialization;
using System.Text.Json;

namespace BlueShadowMon
{
    static public class GameSaver
    {
        public const string SAVE_FILE_NAME = "save.json";
        public const string SAVE_FILE_PATH = "BlueShadowMon/";

        private static readonly JsonSerializerOptions _options =
            new() {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true, 
                PropertyNameCaseInsensitive = true
            };


        public static void SaveGame(Player player)
        {
            // Create json data
            string json = JsonSerializer.Serialize(player, _options);

            // Save json data
            if (!Directory.Exists(SAVE_FILE_PATH))
            {
                Directory.CreateDirectory(SAVE_FILE_PATH);
            }
            File.WriteAllText(SAVE_FILE_PATH + SAVE_FILE_NAME, json);
            Window.Message("Game saved.");
        }

        internal static Player? LoadGame()
        {

            if (!File.Exists(SAVE_FILE_PATH + SAVE_FILE_NAME))
            {
                Window.Message("Save file not found.");
                return null;
            }
            
            // Load json data
            string json = File.ReadAllText(SAVE_FILE_PATH + SAVE_FILE_NAME);

            // Create player from json data
            Player? player = JsonSerializer.Deserialize<Player>(json, _options);

            if (player == null)
            {
                Window.Message("Save file is corrupted.");
            }
            else
            {
                Window.Message("Game loaded.");
            }
            return player;
        }
    }
}
