using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        public const string GameTitle = "Blue Shadow Mon";
        private static int _frameRate = 60;
        private static DateTime _lastFrame = DateTime.Now;
        private static State _currState = State.MainMenu;
        public static State CurrState {
            get { return _currState; }
            set
            {
                Console.Clear();
                _currState = value; 
            }
        } 
        public enum State
        {
            MainMenu = 0,
            Map      = 1,
            Combat   = 2,
            Settings = 3,
        }
        static void Main()
        {
            ConsoleManager.WindowSetup();


            (int x, int y) playerPos = (0, 0);
            Map map = new Map("Map/Map.txt", playerPos);

            // This is the main game loop
            while (true)
            {
                ConsoleManager.CatchInputs();

                // Process inputs
                if (ConsoleManager.Inputs.Count > 0)
                {
                    foreach (ConsoleKeyInfo key in ConsoleManager.Inputs)
                    {
                        switch (CurrState)
                        {
                            case State.MainMenu:
                                Menu.KeyPressed(key.Key);
                                break;
                            case State.Map:
                                map.KeyPressed(key.Key);
                                break;
                            case State.Combat:
                                break;
                            default:
                                break;
                        }
                    }
                }

                // Draw
                switch (CurrState)
                {
                    case State.MainMenu:
                        Menu.DrawMenu();
                        break;
                    case State.Map:
                        map.DrawMap();
                        break;
                    case State.Combat:
                        // Todo: combat scene
                        break;
                    default:
                        break;
                }

                // Wait the next frame
                while (DateTime.Now - _lastFrame < TimeSpan.FromSeconds(1.0 / _frameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}