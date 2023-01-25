using System.Runtime.Versioning;

namespace BlueShadowMon
{

    [SupportedOSPlatform("windows")]
    internal class Menu
    {
        public static ConsoleColor FSelectedColor { get; set; } = Window.DefaultBgColor;
        public static ConsoleColor BSelectedColor { get; set; } = Window.DefaultFgColor;

        protected (Window.ColoredString str, Action callback)[] _items;
        protected int _selectedItem = 0;
        protected ConsoleKey _before;
        protected ConsoleKey _after;
        protected ConsoleKey _confirm;

        public Menu((Window.ColoredString, Action)[] items, ConsoleKey before, ConsoleKey after, ConsoleKey confirm)
        {
            _items = items;
            _items[_selectedItem].str.ForegroundColor = FSelectedColor;
            _items[_selectedItem].str.BackgroundColor = BSelectedColor;
            _before = before;
            _after = after;
            _confirm = confirm;
        }

        public Window.ColoredString this[int num]
        {
            get { return _items[num].str; }
        }

        public int Length
        {
            get { return _items.Length; }
        }

        public void SelectItem(int num)
        {
            if (num < 0)
                num = _items.Length - 1;
            else if (num >= _items.Length)
                num = 0;
            _selectedItem = num;

            for (int i = 0; i < _items.Length; i++)
            {
                if (i == num)
                {
                    _items[i].str.ForegroundColor = FSelectedColor;
                    _items[i].str.BackgroundColor = BSelectedColor;
                }
                else
                {
                    _items[i].str.ForegroundColor = Window.DefaultFgColor;
                    _items[i].str.BackgroundColor = Window.DefaultBgColor;
                }
            }
        }

        public void KeyPressed(ConsoleKey key)
        {
            if (key == _before)
                SelectItem(_selectedItem - 1);
            else if (key == _after)
                SelectItem(_selectedItem + 1);
            else if (key == _confirm)
                _items[_selectedItem].callback();
        }
    }
}
