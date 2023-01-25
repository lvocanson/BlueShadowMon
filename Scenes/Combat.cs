using System.Runtime.Versioning;
using static BlueShadowMon.Menu;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class Combat : Scene
    {
        public static ConsoleColor NameColor { get; set; } = Window.DefaultFgColor;
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
             new Action{ name = "Inventory", fcolor = Window.DefaultFgColor, bcolor = Window.DefaultBgColor },
             new Action{ name = "Pets", fcolor = Window.DefaultFgColor, bcolor = Window.DefaultBgColor },
             new Action{ name = "Flee", fcolor = Window.DefaultFgColor, bcolor = Window.DefaultBgColor },
        };

        public static ConsoleColor FSelectedColor { get; set; } = Window.DefaultBgColor;
        public static ConsoleColor BSelectedColor { get; set; } = Window.DefaultFgColor;

        public static Pet LeftPet { get; set; } = new Pet("Blue Shadow", 1, 50, 50, 10);
        public static Pet RightPet { get; set; } = new Pet("Dark Sasuke", 1, 50, 40, 10);
        
        public override void Draw()
        {
            // Draw statistics
            int leftX = Window.MiddleX / 2;
            int rightX = (int)(Window.MiddleX * 1.5);
            int y;

            // Names
            y = 3;
            Window.Write(LeftPet.Name, leftX, y, NameColor, Window.DefaultBgColor, true);
            Window.Write(RightPet.Name, rightX, y, NameColor, Window.DefaultBgColor, true);

            // Health bars
            y = 4;
            Window.Write(LeftPet.GetHealthBar(20), leftX, y, HealthColor, Window.DefaultBgColor, true);
            Window.Write(RightPet.GetHealthBar(20), rightX, y, HealthColor, Window.DefaultBgColor, true);

            // Draw the actions
            y = Console.WindowHeight - 3;
            for (int i = 0; i < Actions.Length; ++i)
            {
                int x = (int)(Console.WindowWidth * (i + 0.5) / Actions.Length);
                Action choice = Actions[i];
                Window.Write(choice.name, x, y, choice.fcolor, choice.bcolor, true);
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
                    Actions[i].fcolor = Window.DefaultFgColor;
                    Actions[i].bcolor = Window.DefaultBgColor;
                }
            }
        }
        public override void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    SelectedOption = (SelectedOption - 1) % Actions.Length;
                    if (SelectedOption < 0)
                        SelectedOption += Actions.Length;
                    SelectOption(SelectedOption);
                    break;
                case ConsoleKey.RightArrow:
                    SelectedOption = (SelectedOption + 1) % Actions.Length;
                    SelectOption(SelectedOption);
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
