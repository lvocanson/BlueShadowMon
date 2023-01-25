using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal class MenuScene : Scene
    {
        public Menu Menu { get; }
        private string _title;

        public MenuScene(string title, (Window.ColoredString, Action)[] items)
        {
            _title = title;
            Menu = new Menu(items, ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter);
        }
            
        public override void Draw()
        {
            Window.Write(_title, Window.MiddleX, Window.MiddleY - Menu.Length - 2, true);

            for (int i = 0; i < Menu.Length; i++)
            {
                Window.Write(Menu[i], Window.MiddleX, Window.MiddleY - Menu.Length + 2 * i, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            Menu.KeyPressed(key);
        }
    }
}
