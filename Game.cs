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

            // Create menus
            Menu mainMenu = new Menu(new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Play"), () => CurrScene = "Map"),
                (new Window.ColoredString("Combat Test"), () => CurrScene = "Combat"), // REMOVE THIS LATER
                (new Window.ColoredString("Settings"), () => CurrScene = "Settings"),
                (new Window.ColoredString("Exit Game"), () => Environment.Exit(0))
            });

            Menu settingsMenu = new Menu(new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Change Frame Rate"), () => CurrScene = "Frame Rate"),
                (new Window.ColoredString("Change Window Size"), () => CurrScene = "Window Size"),
                (new Window.ColoredString("Back"), () => CurrScene = "Main Menu")
            });

            Menu frameRateMenu = new Menu(new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("30"), () => { FrameRate = 30; CurrScene = "Settings"; }),
                (new Window.ColoredString("60"), () => { FrameRate = 60; CurrScene = "Settings"; }),
                (new Window.ColoredString("120"), () => { FrameRate = 120; CurrScene = "Settings"; }),
                (new Window.ColoredString("144"), () => { FrameRate = 240; CurrScene = "Settings"; }),
                (new Window.ColoredString("240"), () => { FrameRate = 240; CurrScene = "Settings"; }),
                (new Window.ColoredString("Unlimited"), () => { FrameRate = int.MaxValue; CurrScene = "Settings"; })
            });
            frameRateMenu.SelectItem(5);

            Menu windowSizeMenu = new Menu(new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("40%"), () => { Window.Resize(0.4); CurrScene = "Settings"; }),
                (new Window.ColoredString("60%"), () => { Window.Resize(0.6); CurrScene = "Settings"; }),
                (new Window.ColoredString("80%"), () => { Window.Resize(0.8); CurrScene = "Settings"; }),
                (new Window.ColoredString("100%"), () => { Window.Resize(1); CurrScene = "Settings"; })
            });

            // Create Pets
            Pet Max = new Pet("Max", PetType.Dog, new Dictionary<PetStat, int>() {
                { PetStat.Health, 100 },
                { PetStat.PhysicalDamage, 10 },
                { PetStat.MagicalDamage, 10 },
                { PetStat.PhysicalArmor, 10 },
                { PetStat.MagicalArmor, 10 }
            });
            Pet Felix = new Pet("Felix", PetType.Cat, new Dictionary<PetStat, int>() {
                { PetStat.Health, 100 },
                { PetStat.PhysicalDamage, 10 },
                { PetStat.MagicalDamage, 10 },
                { PetStat.PhysicalArmor, 10 },
                { PetStat.MagicalArmor, 10 }
            });
            Pet Python = new Pet("Python", PetType.Snake, new Dictionary<PetStat, int>() {
                { PetStat.Health, 100 },
                { PetStat.PhysicalDamage, 10 },
                { PetStat.MagicalDamage, 10 },
                { PetStat.PhysicalArmor, 10 },
                { PetStat.MagicalArmor, 10 }
            });

            // Create scenes
            _scenes = new Dictionary<string, Scene>()
            {
                {
                    "Main Menu",
                    new MenuScene(new Window.ColoredString(GameTitle, ConsoleColor.Blue, Window.DefaultBgColor), mainMenu)
                },
                {
                    "Map",
                    new Map("Map/Map.txt", (0, 0))
                },
                {
                    "Combat",
                    new MenuScene(new Window.ColoredString(GameTitle, ConsoleColor.Blue, Window.DefaultBgColor), mainMenu)
                },
                {
                    "Settings",
                    new MenuScene(new Window.ColoredString("Settings"), settingsMenu)
                },
                {
                    "Frame Rate",
                    new MenuScene(new Window.ColoredString("Frame Rate"), frameRateMenu)
                },
                {
                    "Window Size",
                    new MenuScene(new Window.ColoredString("Window Size"), windowSizeMenu)
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