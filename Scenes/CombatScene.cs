using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class CombatScene : Scene
    {
        public Pet LeftPet { get; set; }
        public Pet RightPet { get; set; }

        public Menu Menu { get; }

        public CombatScene(Pet leftPet, Pet rightPet)
        {
            LeftPet = leftPet;
            RightPet = rightPet;
            Menu = new Menu(new (Window.ColoredString, Action)[]
            {
                (new Window.ColoredString("Attack", ConsoleColor.Red, Window.DefaultBgColor), () => {
                }),
                (new Window.ColoredString("Inventory", ConsoleColor.Yellow, Window.DefaultBgColor), () => {
                }),
                (new Window.ColoredString("Pets", ConsoleColor.Green, Window.DefaultBgColor), () => {
                }),
                (new Window.ColoredString("Run", ConsoleColor.Blue, Window.DefaultBgColor), () => {
                })
            });
        }

        public void ChangePets(Pet leftPet, Pet rightPet)
        {
            LeftPet = leftPet;
            RightPet = rightPet;
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
