using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Game
    {
        static void Main()
        {
            ConsoleManager.WindowSetup();
            ConsoleManager.WriteText("Blue Shadow Mon", Console.WindowWidth / 2, Console.WindowHeight / 2 - 3, ConsoleColor.Blue, true);
            //Add Menu 
            Menu menu = new();
            Console.WriteLine(Gui.HealthBar(10, 2, 20));
            // This is the main game loop
            while (true)
            {
                // Get inputs
                List<ConsoleKeyInfo> keys = ConsoleManager.CatchInputs();
               
                
                // Process inputs
                if (keys.Count > 0)
                {
                    string inputs = "";
                    foreach (ConsoleKeyInfo key in keys)
                    {
                        inputs += key.Key.ToString() + " ";
                        menu.keyActions(inputs);
                    }
                 
                    ConsoleManager.EraseLine(Console.WindowHeight / 2);
                    //ConsoleManager.WriteText(inputs, Console.WindowWidth / 2, Console.WindowHeight / 2, true);
               
                   

                }
             
                // Update the console
                ConsoleManager.Update();
            }
        }


    }
}