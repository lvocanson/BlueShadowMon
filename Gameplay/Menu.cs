namespace BlueShadowMon
{
    public class Menu
    {
        public static string BeginSelector { get; set; } = "> ";
        public static string EndSelector { get; set; } = " <";

        public Window.ColoredString Title { get; private set; }
        protected (Window.ColoredString str, Action callback)[] _items;
        
        public int SelectedItem { get; private set; } = 0;
        protected Window.ColoredString _selectedItemStr;

        public Menu(Window.ColoredString title, (Window.ColoredString, Action)[] items, int selectedItemNum = 0)
        {
            Title = title;
            if (items.Length == 0)
                throw new ArgumentException("Menu must have at least one item");
            _items = items;
            SelectItem(selectedItemNum);
        }

        public void Init(Window.ColoredString title, (Window.ColoredString, Action)[] items, int? selectedItemNum = null)
        {
            Title = title;
            if (items.Length == 0)
                throw new ArgumentException("Menu must have at least one item");
            _items = items;
            SelectItem(selectedItemNum ?? 0);
        }

        public Window.ColoredString this[int num]
        {
            get
            {
                if (num == SelectedItem)
                    return _selectedItemStr;
                if (num < 0 || num >= _items.Length)
                    throw new IndexOutOfRangeException("Menu index out of range");
                return _items[num].str; 
            }
        }

        public int Length => _items.Length;

        public void SelectItem(int num)
        {
            while (num < 0)
                num += _items.Length;
            while (num >= _items.Length)
                num -= _items.Length;
            SelectedItem = num;
            _selectedItemStr.String = BeginSelector + _items[num].str.String + EndSelector;
            _selectedItemStr.ForegroundColor = _items[num].str.ForegroundColor;
            _selectedItemStr.BackgroundColor = _items[num].str.BackgroundColor;
        }

        public void Before()
        {
            SelectItem(SelectedItem - 1);
        }

        public void After()
        {
            SelectItem(SelectedItem + 1);
        }

        public void Confirm()
        {
            _items[SelectedItem].callback();
        }

        
    }
}
