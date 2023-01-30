namespace BlueShadowMon
{
    internal class CombatScene : Scene
    {
        public Combat Combat { get; private set; }


        public CombatScene(Combat combat)
        {
            Combat = combat;
        }

        public void Init(Combat combat)
        {
            Combat = combat;
        }

        public override void Draw()
        {
            int y = Console.WindowHeight - 3;
            for (int i = 0; i < Combat.CurrentMenu.Length; i++)
            {
                Window.Write(Combat.CurrentMenu[i], (int)(Console.WindowWidth * (i + 0.5) / Combat.CurrentMenu.Length), y, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            Combat.KeyPressed(key);
        }
    }

}
