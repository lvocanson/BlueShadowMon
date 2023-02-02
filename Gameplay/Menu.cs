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

        /// <summary>
        /// Creates a menu with the given title and items.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="items">List of names and actions</param>
        /// <param name="selectedItemNum">Default emplacement of the selector in the menu</param>
        /// <exception cref="ArgumentException"></exception>
        public Menu(Window.ColoredString title, (Window.ColoredString, Action)[] items, int selectedItemNum = 0)
        {
            Title = title;
            if (items.Length == 0)
                throw new ArgumentException("Menu must have at least one item");
            _items = items;
            SelectItem(selectedItemNum);
        }

        /// <summary>
        /// Recycles the menu with the given title and items.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="items">List of names and actions</param>
        /// <param name="selectedItemNum">Default emplacement of the selector in the menu</param>
        /// <exception cref="ArgumentException"></exception>
        public void Init(Window.ColoredString title, (Window.ColoredString, Action)[] items, int? selectedItemNum = null)
        {
            Title = title;
            if (items.Length == 0)
                throw new ArgumentException("Menu must have at least one item");
            _items = items;
            SelectItem(selectedItemNum ?? 0);
        }

        /// <summary>
        /// Returns the item at the given index.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
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

        /// <summary>
        /// Put the menu selector at the given index.
        /// </summary>
        /// <param name="num"></param>
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

        /// <summary>
        /// Put the selector on the previous item.
        /// If it was at the first item, the selector is moved to the last.
        /// </summary>
        public void Before()
        {
            SelectItem(SelectedItem - 1);
        }

        /// <summary>
        /// Put the selector on the next item.
        /// If it was at the last item, the selector is moved to the first.
        /// </summary>
        public void After()
        {
            SelectItem(SelectedItem + 1);
        }

        /// <summary>
        /// Call the action of the selected item.
        /// </summary>
        public void Confirm()
        {
            _items[SelectedItem].callback();
        }
    }
}
