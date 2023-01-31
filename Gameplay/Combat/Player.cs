
namespace BlueShadowMon
{
    public class Player
    {
        public Dictionary<Consumable, int> Inventory { get; private set; } = new();
        public List<Pet> Pets { get; } = new();
        
        public (int x, int y ) MapPosition { get; private set; }
        public int x { get => MapPosition.x; }
        public int y { get => MapPosition.y; }


        public Player((int x, int y) mapPosition)
        {
            MapPosition = mapPosition;
        }

        public void Move(int x, int y)
        {
            MapPosition = (x, y);
        }
        
        public void Move((int x, int y) playerPos)
        {
            MapPosition = playerPos;
        }
        
        public void AddPet(Pet newPet)
        {
            Pets.Add(newPet);
        }
        
        public void RemovePet(Pet pet)
        {
            if (Pets.Contains(pet))
            {
                Pets.Remove(pet);
            }
        }

        public void AddConsumable(Consumable consumable, int amount)
        {
            Inventory.Add(consumable, amount);
        }

        public void RemoveConsumable(Consumable consumable)
        {
            if (Inventory[consumable].Equals(0))
            {
                Inventory.Remove(consumable);
            }
        }

        public Menu CreateInventoryMenu()
        {
            List<(Window.ColoredString, Action)> actions = new();
            
            foreach (Consumable c in Inventory.Keys)
            {
                Window.ColoredString name = c.Name;
                name.String = $"{Inventory[c]} - {name.String}";
                actions.Add((name, () => {
                    // TODO: Use the consumable
                }));
            }

            if (actions.Count == 0)
            {
                actions.Add((new Window.ColoredString("- Empty -"), () => { }));
            }
            
            return new Menu(new Window.ColoredString("Inventory", ConsoleColor.Yellow, Window.DefaultBgColor), actions.ToArray());
        }
    }
}
