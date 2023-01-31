
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
            int y = Console.WindowHeight - 3;
            Window.Write(_inventoryMenu.Title, Window.MiddleX, y - 2, true);
            for (int i = 0; i < _inventoryMenu.Length; i++)
            {
                Window.Write(_inventoryMenu[i], (int)(Console.WindowWidth * (i + 0.5) / _inventoryMenu.Length), y, true);
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
                    Console.Clear();
                    _inventoryMenu.SelectItem(0);
                    Game.ToggleInventory();
                    break;
            }
        }

    }
    
}
