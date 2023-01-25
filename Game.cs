using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
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

        public static Dictionary<string, Scene> Scenes { get; private set; }
        private static string _currScene = "Menu";
        public static string CurrScene
        {
            get { return _currScene; }
            set
            {
                if (!Scenes.ContainsKey(value))
                    throw new ArgumentException("Scene does not exist");

                Console.BackgroundColor = Window.DefaultBgColor;
                Console.Clear();
                _currScene = value;
            }
        }
        static void Main()
        {
            Window.Setup();

            // Create scenes
            Scenes = new Dictionary<string, Scene>()
            {
                {
                    "Main Menu",
                    new MenuScene(new Window.ColoredString(GameTitle, ConsoleColor.Blue, Window.DefaultBgColor), Data.MainMenu)
                },
                {
                    "Map",
                    new Map("Map/Map.txt", (0, 0))
                },
                {
                    "Combat",
                    new CombatScene()
                },
                {
                    "Settings",
                    new MenuScene(new Window.ColoredString("Settings"), Data.SettingsMenu)
                },
                {
                    "Frame Rate",
                    new MenuScene(new Window.ColoredString("Frame Rate"), Data.FrameRateMenu)
                },
                {
                    "Window Size",
                    new MenuScene(new Window.ColoredString("Window Size"), Data.WindowSizeMenu)
                },
            };

            // This is the main game loop
            CurrScene = "Main Menu";
            while (true)
            {
                Window.CatchInputs();

                // Process inputs
                if (Window.Inputs.Count > 0)
                {
                    foreach (ConsoleKeyInfo kInfo in Window.Inputs)
                    {
                        Scenes[_currScene].KeyPressed(kInfo.Key);
                    }
                }

                // Draw
                Scenes[_currScene].Draw();

                // Wait the next frame
                while (DateTime.Now - _lastFrame < TimeSpan.FromSeconds(1.0 / _frameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}