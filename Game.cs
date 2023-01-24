using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        private static int FrameRate = 60;
        private static DateTime LastFrame = DateTime.Now;
        
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
                    }
                }

                // Draw
                map.Draw(playerPos.X, playerPos.Y);

                // Wait the next frame
                while (DateTime.Now - LastFrame < TimeSpan.FromSeconds(1.0 / FrameRate))
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}