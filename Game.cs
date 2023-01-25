using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        public const string GameTitle = "Blue Shadow Mon";
        private static int _frameRate = 60;
        private static DateTime _lastFrame = DateTime.Now;

        private static Dictionary<string, Scene> _scenes;
        private static string _currScene;
        public static string CurrScene
        {
            get { return _currScene; }
            set
            {
                if (!_scenes.ContainsKey(value))
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
            _scenes = new Dictionary<string, Scene>()
            {
                {
                    "Main Menu",
                    new MenuScene( new Window.ColoredString(GameTitle, ConsoleColor.Blue, Window.DefaultBgColor), new (Window.ColoredString, Action)[] {
                        (new Window.ColoredString("Play"), () => CurrScene = "Map"),
                        (new Window.ColoredString("Combat Test"), () => CurrScene = "Combat"), // REMOVE THIS LATER
                        (new Window.ColoredString("Exit"), () => Environment.Exit(0))
                }) },
                {
                    "Map",
                    new Map("Map/Map.txt", (0, 0))
                },
                {
                    "Combat",
                    new Combat()
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
                        _scenes[_currScene].KeyPressed(kInfo.Key);
                    }
                }

                // Draw
                _scenes[_currScene].Draw();

                // Wait the next frame
                while (DateTime.Now - _lastFrame < TimeSpan.FromSeconds(1.0 / _frameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}