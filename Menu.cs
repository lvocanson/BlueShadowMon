using System.Runtime.Versioning;

namespace BlueShadowMon
{

    [SupportedOSPlatform("windows")]
    internal class Menu
    {
        public static string BeginSelector { get; set; } = "> ";
        public static string EndSelector { get; set; } = " <";
        public static ConsoleColor FSelectedColor { get; set; } = Window.DefaultBgColor;
        public static ConsoleColor BSelectedColor { get; set; } = Window.DefaultFgColor;

        protected (Window.ColoredString str, Action callback)[] _items;
        protected int _selectedItemNum = 0;
        protected Window.ColoredString _selectedItemStr;

        public Menu((Window.ColoredString, Action)[] items)
        {
            _items = items;
            _selectedItemStr = new Window.ColoredString(BeginSelector + _items[_selectedItemNum].str.String + EndSelector);
        }

        public Window.ColoredString this[int num]
        {
            get
            {
                if (num == _selectedItemNum)
                    return _selectedItemStr;
                return _items[num].str; 
            }
        }

        public int Length
        {
            get { return _items.Length; }
        }

        public void SelectItem(int num)
        {
            Console.Clear();
            if (num < 0)
                num = _items.Length - 1;
            else if (num >= _items.Length)
                num = 0;
            _selectedItemNum = num;
            _selectedItemStr.String = BeginSelector + _items[num].str.String + EndSelector;
        }

        public void Before()
        {
            SelectItem(_selectedItemNum - 1);
        }

        public void After()
        {
            SelectItem(_selectedItemNum + 1);
        }

        public void Confirm()
        {
            _items[_selectedItemNum].callback();
        }
    }
}
