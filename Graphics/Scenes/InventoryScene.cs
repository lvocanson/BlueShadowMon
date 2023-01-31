
namespace BlueShadowMon
{
     class InventoryScene : Scene
    {
        public Inventory Inventory { get; private set; }

        public InventoryScene(Inventory inventory)
        {
            Inventory = inventory;
        }

        public void Init(Inventory inventory)
        {
            Inventory = inventory;
        }

        public override void Draw()
        {
            int y = Console.WindowHeight - 3;
            Window.Write(Inventory.CurrentMenu.Title, Window.MiddleX, y - 2, true);
            for (int i = 0; i < Inventory.CurrentMenu.Length; i++)
            {
                Window.Write(Inventory.CurrentMenu[i], (int)(Console.WindowWidth * (i + 0.5) / Inventory.CurrentMenu.Length), y, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            Inventory.KeyPressed(key);
        }

    }
    
}
