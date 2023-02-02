namespace BlueShadowMon
{
    internal class MenuScene : Scene
    {
        public Menu Menu { get; private set; }

        /// <summary>
        /// Creates a new menu scene.
        /// </summary>
        /// <param name="menu">Menu to display</param>
        public MenuScene(Menu menu)
        {
            Menu = menu;
        }

        /// <summary>
        /// Recycle the scene with a new menu.
        /// </summary>
        /// <param name="menu"></param>
        public void Init(Menu menu)
        {
            Menu = menu;
        }

        public override void Draw()
        {
            Window.Write(Menu.Title, Window.MiddleX, Window.MiddleY - Menu.Length - 3, true);

            for (int i = 0; i < Menu.Length; i++)
            {
                Window.Write(Menu[i], Window.MiddleX, Window.MiddleY - Menu.Length + 2 * i, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    Menu.Before();
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    Menu.After();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Menu.Confirm();
                    break;
            }
        }
    }
}
