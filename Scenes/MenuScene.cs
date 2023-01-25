using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class MenuScene : Scene
    {
        public Menu Menu { get; }
        private Window.ColoredString _title;

        public MenuScene(Window.ColoredString title, Menu menu)
        {
            _title = title;
            Menu = menu;
        }

        public MenuScene(Window.ColoredString title, (Window.ColoredString, Action)[] items)
        {
            _title = title;
            Menu = new Menu(items);
        }

        public override void Draw()
        {
            Window.Write(_title, Window.MiddleX, Window.MiddleY - Menu.Length - 3, true);

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
                    Menu.Before();
                    break;
                case ConsoleKey.DownArrow:
                    Menu.After();
                    break;
                case ConsoleKey.Enter:
                    Menu.Confirm();
                    break;
            }
        }
    }
}
