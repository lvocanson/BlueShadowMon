namespace BlueShadowMon
{
    public static class Game
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

        // Player
        private static Player _player { get; set; }
        public const int DEFAULT_X_POS_ON_MAP = 389;
        public const int DEFAULT_Y_POS_ON_MAP = 50;
        public const string DEFAULT_MAP_PATH = "Map/Map.txt";

        // NPC
        private static NPC[] _npcs { get; set; } = new NPC[]
        {
            new NPC(391, 50, new string[]
            {
                "- Greatings, young trainer! I'll teach you the basics.",
                "You can explore the world as you want!" + Environment.NewLine + "But be careful, you can encounter wild monsters in the grass!",
                "Press I to open your inventory." + Environment.NewLine + "You can use it to store your items.",
                "Press ESC to open the menu." + Environment.NewLine + "You can save, quit or access the settings from there.",
                "Well then, good luck on your journey!" + Environment.NewLine,
            }),
            new NPC(370, 74, new string[] 
            {
                "- Hi! I'm Bob. I'm useless.",
                "- Hi. I'm a Pet trainer.",
                "- Ok."
            }),
            new NPC(398, 82, new string[]
            {
                "- Hi! I'm Terry. I'm supposed to be a merchant, but y'know..." + Environment.NewLine + "Developer's laziness...",
                "- Hmm..." + Environment.NewLine + "Then, what are you doing here?",
                "- I'm supposed to be a merchant, but I'm not." + Environment.NewLine + "So, I'm just here to waste your time.",
            }),
            new NPC(42, 104, new string[]
            {
               "- Wow! You made it this far!" + Environment.NewLine + "You're a real hero!",
               "- I'm not a hero." + Environment.NewLine + "I'm training my Pets.",
               "- Oh, you're training to become one, I see." + Environment.NewLine + "Then, I'll gift you this.",
               "(He gave you nothing.)",
               "- . . ." + Environment.NewLine + "Uh... Thanks?",
            })
        };

        // Scene Manager
        private static MenuScene _menuScene;
        private static MapScene _mapScene;
        private static CombatScene _combatScene;
        private static InventoryScene _inventoryScene;
        private static Scene _currScene;
        private static Scene _previousScene;
        public static Scene CurrScene
        {
            get => _currScene;
            private set
            {
                Console.BackgroundColor = Window.DefaultBgColor;
                Console.Clear();
                _previousScene = _currScene;
                _currScene = value;
            }
        }

        public static void SwitchToMenuScene() => CurrScene = _menuScene;
        public static void SwitchToMenuScene(string menuName)
        {
            if (!Menus.ContainsKey(menuName))
                throw new ArgumentException("Menu does not exist");
            _menuScene.Init(Menus[menuName]);
            CurrScene = _menuScene;
        }
        public static void SwitchToMapScene() => CurrScene = _mapScene;
        public static void SwitchToMapScene(string path, (int x, int y) playerPos)
        {
            _player.Move(playerPos);
            _mapScene.Init(new Map(path, _player, _npcs));
            CurrScene = _mapScene;
        }
        public static void SwitchToCombatScene() => CurrScene = _combatScene;
        public static void SwitchToCombatScene(List<Pet> enemies)
        {
            _combatScene.Init( new Combat(_player, enemies));
            CurrScene = _combatScene;
        }
        public static void ToggleInventory()
        {
            if (_currScene == _inventoryScene)
                CurrScene = _previousScene;
            else
                CurrScene = _inventoryScene;
        }

        /// <summary>
        /// Initialize the game
        /// </summary>
        static Game()
        {
            _player = new Player((DEFAULT_X_POS_ON_MAP, DEFAULT_Y_POS_ON_MAP));
            _menuScene = new MenuScene(Menus["Main Menu"]);
            _currScene = _menuScene;
            _mapScene = new MapScene(new Map(DEFAULT_MAP_PATH, _player, _npcs));
            _combatScene = new CombatScene(new Combat(_player, new List<Pet>()));
            _inventoryScene = new InventoryScene(_player);
            _previousScene = _menuScene;
        }

        /// <summary>
        /// Entry point of the program. Will lunch
        /// the game and handle everything.
        /// </summary>
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

        // Menus
        private static Dictionary<string, Menu> Menus { get; } = new Dictionary<string, Menu>()
        {
            {
                "Main Menu", new Menu(new Window.ColoredString(GameTitle, ConsoleColor.Blue, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Play"), () => SwitchToMapScene()),
                (new Window.ColoredString("Load Save"), () => { _player = GameSaver.LoadGame() ?? _player!; }),
                (new Window.ColoredString("Save Game"), () => GameSaver.SaveGame(_player!)),
                (new Window.ColoredString("Settings"), () => SwitchToMenuScene("Settings")),
                (new Window.ColoredString("Save and Exit Game"), () => { GameSaver.SaveGame(_player!); Window.Quit(); })
            }) },
            {
                "Settings", new Menu(new Window.ColoredString("Settings", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Change Frame Rate"), () => SwitchToMenuScene("Frame Rate")),
                (new Window.ColoredString("Change Window Size"), () => SwitchToMenuScene("Window Size")),
                (new Window.ColoredString("Back"), () => SwitchToMenuScene("Main Menu")),
            }) },
            {
                "Frame Rate", new Menu(new Window.ColoredString("Frame Rate", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("30"), () => { FrameRate = 30; SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("60"), () => { FrameRate = 60; SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("120"), () => { FrameRate = 120; SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("144"), () => { FrameRate = 240; SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("240"), () => { FrameRate = 240; SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("Unlimited"), () => { FrameRate = int.MaxValue; SwitchToMenuScene("Settings"); })
            }, 5) },
            {
                "Window Size", new Menu(new Window.ColoredString("Window Size", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("50%"), () => { Window.Resize(0.5F); SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("60%"), () => { Window.Resize(0.6F); SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("70%"), () => { Window.Resize(0.7F); SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("80%"), () => { Window.Resize(0.8F); SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("90%"), () => { Window.Resize(0.9F); SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("100%"), () => { Window.Resize(1); SwitchToMenuScene("Settings"); })
            }, 2) }
        };
    }
}