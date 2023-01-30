namespace BlueShadowMon.Gameplay.Combat
{
    public class Combat
    {
        public Pet[] Allies { get; private set; }
        public Pet[] Enemies { get; private set; }
        public Menu CurrentMenu { get; private set; }

        private Menu CombatMenu => new Menu(
            new Window.ColoredString("What do you want to do?", ConsoleColor.Red, Window.DefaultBgColor),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack"), () => { }),
                (new Window.ColoredString("Inventory"), () => { }),
                (new Window.ColoredString("Pets"), () => { }),
                (new Window.ColoredString("Run"), () => { Game.SwitchToMapScene(); })
            });

        public Combat(Pet[] allies, Pet[] enemies)
        {
            CurrentMenu = CombatMenu;
            Allies = allies;
            Enemies = enemies;
        }

        public void KeyPressed(ConsoleKey key)
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
