using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        public static string GameTitle { get; set; } = "Blue Shadow Mon";
        private static int FrameRate = 60;
        private static DateTime LastFrame = DateTime.Now;
        public static State CurrState { get; set; } = State.Menu;

        public enum State
        {
            Menu     = 0,
            Map      = 1,
            Combat   = 2,
            Settings = 3,
        }
        static void Main()
        {
            ConsoleManager.WindowSetup();


            Map map = new Map("Map/Map.txt");
            var playerPos = (X: 0, Y: 0);

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
                            case State.Menu:
                                Menu.KeyPressed(key.Key);
                                break;
                            case State.Map:
                                switch (key.Key)
                                {
                                    case ConsoleKey.UpArrow:
                                        playerPos.Y--;
                                        break;
                                    case ConsoleKey.DownArrow:
                                        playerPos.Y++;
                                        break;
                                    case ConsoleKey.LeftArrow:
                                        playerPos.X--;
                                        break;
                                    case ConsoleKey.RightArrow:
                                        playerPos.X++;
                                        break;
                                }
                                break;
                            case State.Combat:
                                Combat.KeyPressed(key.Key);
                                break;
                            default:
                                break;
                        }
                    }
                }

                // Draw
                switch (CurrState)
                {
                    case State.Menu:
                        Menu.DrawMenu();
                        break;
                    case State.Map:
                        map.Draw(playerPos.X, playerPos.Y);
                        break;
                    case State.Combat:
                        Combat.DrawCombat();
                        break;
                    default:
                        break;
                }

                // Wait the next frame
                while (DateTime.Now - LastFrame < TimeSpan.FromSeconds(1.0 / FrameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }


    }
}