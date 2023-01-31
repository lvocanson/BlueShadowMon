
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
            int topY = Math.Max(Window.MiddleY - _inventoryMenu.Length - 2, 1);
            Window.Write(_inventoryMenu.Title, Window.MiddleX, topY, true);

            // Max height of the menu to display
            int height = Math.Min(_inventoryMenu.Length, Console.WindowHeight / 2 - 2);
            
            for (int i = 0; i < height; i++)
            {
                Window.Write(_inventoryMenu[i], Window.MiddleX, Window.MiddleY - _inventoryMenu.Length + i * 2, true);
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
