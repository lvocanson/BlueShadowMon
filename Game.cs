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
        private static Player _player;

        // Scene Manager
        private static MenuScene _menuScene;
        private static MapScene _mapScene;
        private static CombatScene _combatScene;
        private static Scene _currScene;
        public static Scene CurrScene
        {
            get => _currScene;
            private set
            {
                Console.BackgroundColor = Window.DefaultBgColor;
                Console.Clear();
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
            _mapScene.Init(new Map(path, _player));
            CurrScene = _mapScene;
        }
        public static void SwitchToCombatScene() => CurrScene = _combatScene;
        public static void SwitchToCombatScene(List<Pet> allies, List<Pet> ennemies)
        {
            _combatScene.Init(new Combat(allies, ennemies));
            CurrScene = _combatScene;
        }

        static Game()
        {
            _player = new Player((285, 53));
            _menuScene = new MenuScene(Menus["Main Menu"]);
            _currScene = _menuScene;
            _mapScene = new MapScene(new Map("Map/Map.txt", _player));
            _combatScene = new CombatScene(new Combat(
                new(){
                    new Pet("MyCat", PetType.Cat, Data.StarterStats, Data.StarterIncrements),
                    new Pet("MyDog", PetType.Dog, Data.StarterStats, Data.StarterIncrements),
                    new Pet("MySnake", PetType.Snake, Data.StarterStats, Data.StarterIncrements)
                },
                new(){
                    new Pet("EnemyCat", PetType.Cat, Data.StarterStats, Data.StarterIncrements),
                    new Pet("EnemyDog", PetType.Dog, Data.StarterStats, Data.StarterIncrements),
                    new Pet("EnemySnake", PetType.Snake, Data.StarterStats, Data.StarterIncrements)
                }
            ));
        }

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
                (new Window.ColoredString("Combat Test"), () => { SwitchToCombatScene(); }), // REMOVE THIS LATER
                (new Window.ColoredString("Settings"), () => SwitchToMenuScene("Settings")),
                (new Window.ColoredString("Exit Game"), () => Window.Quit())
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