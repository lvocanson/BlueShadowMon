using Newtonsoft.Json;

namespace BlueShadowMon
{
    static public class GameSaver
    {
        public const string SAVE_FILE_NAME = "save.json";
        public const string SAVE_FILE_PATH = "BlueShadowMon/";

        private static readonly JsonSerializerSettings _options =
            new()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };


        public static void SaveGame(Player player)
        {
            // Create json data
            string json = JsonConvert.SerializeObject(player, _options);

            // Save json data
            if (!Directory.Exists(SAVE_FILE_PATH))
            {
                Directory.CreateDirectory(SAVE_FILE_PATH);
            }
            File.WriteAllText(SAVE_FILE_PATH + SAVE_FILE_NAME, json);
            Window.Message("Game saved.", true);
        }

        internal static Player? LoadGame()
        {

            if (!File.Exists(SAVE_FILE_PATH + SAVE_FILE_NAME))
            {
                Window.Message("Save file not found.", true);
                return null;
            }

            // Load json data
            string json = File.ReadAllText(SAVE_FILE_PATH + SAVE_FILE_NAME);

            // Create player from json data
            Player player;
            try
            {
                player = JsonConvert.DeserializeObject<Player>(json, _options);
            }
            catch (Exception e)
            {
                Window.Message("Something went wrong while loading the save file.\n" + e, true);
                return null;
            }

            Window.Message("Game loaded.", true);
            return player;
        }
    }
}
