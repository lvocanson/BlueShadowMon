namespace BlueShadowMon
{
    internal class CombatScene : Scene
    {
        public Menu CurrentMenu { get; private set; }
        private Menu CombatMenu => new Menu(
            new Window.ColoredString("What do you want to do?", ConsoleColor.Red, Window.DefaultBgColor),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack"), () => { }),
                (new Window.ColoredString("Inventory"), () => { }),
                (new Window.ColoredString("Pets"), () => { }),
                (new Window.ColoredString("Run"), () => { Game.SwitchToMapScene(); })
            });
        
        public CombatScene()
        {
            CurrentMenu = CombatMenu;
        }
        
        public override void Draw()
        {
            int y = Console.WindowHeight - 3;
            for (int i = 0; i < CurrentMenu.Length; i++)
            {
                Window.Write(CurrentMenu[i], (int)(Console.WindowWidth * (i + 0.5) / CurrentMenu.Length), y, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    CurrentMenu.Before();
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                    CurrentMenu.After();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    CurrentMenu.Confirm();
                    break;
                case ConsoleKey.Escape:
                    CurrentMenu.SelectItem(0);
                    CurrentMenu = CombatMenu;
                    break;
                default:
                    break;
            }
        }
    }

}
