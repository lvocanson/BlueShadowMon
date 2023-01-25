using System.Runtime.Versioning;

namespace BlueShadowMon
{

    [SupportedOSPlatform("windows")]
    public class Menu
    {
        public static string BeginSelector { get; set; } = "> ";
        public static string EndSelector { get; set; } = " <";

        public Window.ColoredString Title { get; private set; }
        protected (Window.ColoredString str, Action callback)[] _items;
        protected int _selectedItemNum;
        protected Window.ColoredString _selectedItemStr;

        public Menu(Window.ColoredString title, (Window.ColoredString, Action)[] items, int selectedItemNum = 0)
        {
            Title = title;
            _items = items;
            _selectedItemNum = selectedItemNum;
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
