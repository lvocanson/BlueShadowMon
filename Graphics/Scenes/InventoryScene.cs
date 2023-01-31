
namespace BlueShadowMon
{
     class InventoryScene : Scene
    {
        public Player Player { get; private set; }
        private Menu _inventoryMenu;

        public InventoryScene(Player _player)
        {
            Player = _player;
            _inventoryMenu = Player.CreateInventoryMenu();
        }
        
        public override void Draw()
        {

            // Max height of the menu to display
            int height = Math.Min(_inventoryMenu.Length, Console.WindowHeight / 2 - 2);
            Window.Write(_inventoryMenu.Title, Window.MiddleX, Window.MiddleY - height - 2, true);

            // For scroll
            int offset = 0;
            if (_inventoryMenu.SelectedItem > height / 2 && _inventoryMenu.Length - _inventoryMenu.SelectedItem > height / 2)
            {
                offset = _inventoryMenu.SelectedItem - height / 2;
            }
            else if (_inventoryMenu.Length - _inventoryMenu.SelectedItem <= height / 2)
            {
                offset = _inventoryMenu.Length - height;
            }

            for (int i = 0; i < height; i++)
            {
                Window.Write(_inventoryMenu[i + offset], Window.MiddleX, Window.MiddleY - height + i * 2, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    _inventoryMenu.Before();
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    _inventoryMenu.After();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    _inventoryMenu.Confirm();
                    break;
                    case ConsoleKey.Escape:
                    _inventoryMenu.SelectItem(0);
                    Game.ToggleInventory();
                    break;
            }
        }

    }
    
}
