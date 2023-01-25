using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        // Window settings
        public const string GameTitle = "Blue Shadow Mon";
        private static int _frameRate = int.MaxValue;
        public static int FrameRate
        {
            get { return _frameRate; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Frame rate must be greater than 0");
                _frameRate = value;
            }
        }
        private static DateTime _lastFrame = DateTime.Now;

        // Scene Manager
        private static MenuScene _menuScene = new MenuScene(Data.Menus["Main Menu"]);
        private static MapScene mapScene = new MapScene("Map/Map.txt", (0, 0));
        private static CombatScene combatScene = new CombatScene();
        private static Scene _currScene = _menuScene;
        public static Scene CurrScene
        {
            get => _currScene;
            set
            {
                Console.BackgroundColor = Window.DefaultBgColor;
                Console.Clear();
                _currScene = value;
            }
        }

        public static void SwitchToMenuScene() => CurrScene = _menuScene;
        public static void SwitchToMenuScene(string menuName)
        {
            if (!Data.Menus.ContainsKey(menuName))
                throw new ArgumentException("Menu does not exist");
            _menuScene.Init(Data.Menus[menuName]);
            CurrScene = _menuScene;
        }
        public static void SwitchToMapScene() => CurrScene = mapScene;
        public static void SwitchToMapScene(string path, (int x, int y) playerPos)
        {
            mapScene.Init(path, playerPos);
            CurrScene = mapScene;
        }
        public static void SwitchToCombatScene() => CurrScene = combatScene;


        static void Main()
        {
            Window.Setup();

            // This is the main game loop
            while (true)
            {
                Window.CatchInputs();

                // Process inputs
                if (Window.Inputs.Count > 0)
                {
                    foreach (ConsoleKeyInfo kInfo in Window.Inputs)
                    {
                        _currScene.KeyPressed(kInfo.Key);
                    }
                }

                // Draw
                _currScene.Draw();

                // Wait the next frame
                while (DateTime.Now - _lastFrame < TimeSpan.FromSeconds(1.0 / _frameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}