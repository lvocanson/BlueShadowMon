using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class CombatScene : Scene
    {
        public Menu Menu { get; }

        public CombatScene(Menu menu)
        {
            Menu = menu;
        }
        public override void Draw()
        {

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
