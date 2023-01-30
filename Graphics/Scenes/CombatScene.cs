namespace BlueShadowMon
{
    internal class CombatScene : Scene
    {
        public Menu Menu { get; } = new Menu(
            new Window.ColoredString("What do you want to do?", ConsoleColor.Red, Window.DefaultBgColor),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack"), () => { }),
                (new Window.ColoredString("Inventory"), () => { }),
                (new Window.ColoredString("Pets"), () => { }),
                (new Window.ColoredString("Run"), () => { })
            });
        
        public CombatScene()
        {
            ;
        }
        public override void Draw()
        {
            int y = Console.WindowHeight - 3;
            for (int i = 0; i < Menu.Length; i++)
            {
                Window.Write(Menu[i], (int)(Console.WindowWidth * (i + 0.5) / Menu.Length), y, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    Menu.Before();
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                    Menu.After();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Menu.Confirm();
                    break;
                case ConsoleKey.Escape:
                    Game.SwitchToCombatScene();
                    break;
                default:
                    break;
            }
        }
    }

}
