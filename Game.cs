using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        static void Main()
        {
            ConsoleManager.WindowSetup();
            ConsoleManager.WriteText("Blue Shadow Mon", Console.WindowWidth / 2, Console.WindowHeight / 2, true);

            // Create a new map
            Map map = new Map(5, 5);
            ConsoleManager.PrintMap(map, 3, 2);

            // This is the main game loop
            while (true)
            {
                
                // Update the console
                ConsoleManager.Update();
            }
        }
    }
}