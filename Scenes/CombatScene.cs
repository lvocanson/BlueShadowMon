using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class CombatScene : Scene
    {
        public Menu Menu { get; } = new Menu(new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack"), () => Game.CurrScene = "Ability"),
                (new Window.ColoredString("Inventory"), () => Game.CurrScene = "Inventory"),
                (new Window.ColoredString("Pets"), () => Game.CurrScene = "Pets"),
                (new Window.ColoredString("Run"), () => Game.CurrScene = "Map")
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
                    Menu.Before();
                    break;
                case ConsoleKey.RightArrow:
                    Menu.After();
                    break;
                case ConsoleKey.Enter:
                    Menu.Confirm();
                    break;
                case ConsoleKey.Escape:
                    Game.CurrScene = "Combat";
                    break;
                default:
                    break;
            }
        }
    }

}
