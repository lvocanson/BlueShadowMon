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

            // This is the main game loop
            while (true)
            {
                
                // Update the console
                ConsoleManager.Update();
            }
        }
    }
}