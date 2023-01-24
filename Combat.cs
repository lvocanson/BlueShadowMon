using System.Runtime.Versioning;
using static BlueShadowMon.Menu;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class Combat
    {
        public static ConsoleColor NameColor { get; set; } = ConsoleManager.DefaultFgColor;
        public static ConsoleColor HealthColor { get; set; } = ConsoleColor.Green;

        private static int SelectedOption { get; set; } = 0;

        public struct Action
        {
            public string name;
            public ConsoleColor fcolor;
            public ConsoleColor bcolor;
        }

        private static Action[] Actions = new Action[]
        {
             new Action{ name = "Attack", fcolor = FSelectedColor, bcolor = BSelectedColor },
             new Action{ name = "Inventory", fcolor = FSelectedColor, bcolor = BSelectedColor },
             new Action{ name = "Pets", fcolor = FSelectedColor, bcolor = BSelectedColor },
             new Action{ name = "Flee", fcolor = FSelectedColor, bcolor = BSelectedColor },
        };

        public static ConsoleColor FSelectedColor { get; set; } = ConsoleManager.DefaultBgColor;
        public static ConsoleColor BSelectedColor { get; set; } = ConsoleManager.DefaultFgColor;

        public static Pet LeftPet { get; set; } = new Pet("Blue Shadow", 1, 50, 50, 10);
        public static Pet RightPet { get; set; } = new Pet("Dark Sasuke", 1, 50, 40, 10);
        
        public static void DrawCombat()
        {
            // Draw statistics
            int leftX = ConsoleManager.MiddleX / 2;
            int rightX = (int)(ConsoleManager.MiddleX * 1.5);
            int y;

            // Names
            y = 3;
            ConsoleManager.WriteText(LeftPet.Name, leftX, y, NameColor, ConsoleManager.DefaultBgColor, true);
            ConsoleManager.WriteText(RightPet.Name, rightX, y, NameColor, ConsoleManager.DefaultBgColor, true);

            // Health bars
            y = 4;
            ConsoleManager.WriteText(LeftPet.GetHealthBar(20), leftX, y, HealthColor, ConsoleManager.DefaultBgColor, true);
            ConsoleManager.WriteText(RightPet.GetHealthBar(20), rightX, y, HealthColor, ConsoleManager.DefaultBgColor, true);

            // Draw the actions
            y = Console.WindowHeight - 3;
            for (int i = 0; i < Actions.Length; i++)
            {
                int x = (int)(Console.WindowWidth * (i + 0.5) / Actions.Length);
                ConsoleManager.WriteText(Actions[i], x, y, ConsoleManager.DefaultFgColor, ConsoleManager.DefaultBgColor, true);
            }            
        }

        public static void SelectOption(int option)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                if (i == option)
                {
                    Actions[i].fcolor = FSelectedColor;
                    Actions[i].bcolor = BSelectedColor;

                }
                else
                {
                    Actions[i].fcolor = ConsoleManager.DefaultFgColor;
                    Actions[i].bcolor = ConsoleManager.DefaultBgColor;
                }
            }
        }
        public static void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    SelectedOption = (SelectedOption - 1) % Actions.Length;
                    SelectOption(SelectedOption);
                    break;
                case ConsoleKey.RightArrow:
                    SelectedOption = (SelectedOption + 1) % Actions.Length;
                    break;
                case ConsoleKey.Enter:
                    switch (Actions[SelectedOption].name)
                    {
                        case "Attack":
                            // TODO Print 4 abilities
                            break;
                        case "Inventory":
                            // TODO Print Inventory
                            break;
                        case "Pets":
                            // TODO Print the team
                            break;
                        case "Flee":
                            // TODO Back to the map
                            break;
                        default:
                            break;
                    }
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    break;
            }
        }
    }

}
