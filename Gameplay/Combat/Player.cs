namespace BlueShadowMon
{
    public class Player
    {
        const int MAX_PETS = 3;
        public Dictionary<Consumable, int> Inventory { get; private set; } = new();
        public Pet?[] Pets { get; }
        
        public (int x, int y ) MapPosition { get; private set; }
        public int x { get => MapPosition.x; }
        public int y { get => MapPosition.y; }

        public Player((int x, int y) mapPosition)
        {
            Pets = new Pet?[MAX_PETS] { null, null, null };
            MapPosition = mapPosition;
        }

        /// <summary>
        /// Move the player to the specified x and y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(int x, int y)
        {
            MapPosition = (x, y);
        }
        
        /// <summary>
        /// Move the player to the specified position.
        /// </summary>
        /// <param name="playerPos"></param>
        public void Move((int x, int y) playerPos)
        {
            MapPosition = playerPos;
        }
        
        /// <summary>
        /// Add a Pet to the player's team.
        /// </summary>
        /// <param name="newPet"></param>
        /// <exception cref="Exception"></exception>
        public void AddPet(Pet newPet)
        {
            if (Pets.Count() >= MAX_PETS)
                throw new Exception($"Maximum number of Pets ({MAX_PETS}) exceeded.");
            for (int i = 0; i < MAX_PETS; i++)
            {
                if (Pets[i] == null)
                {
                    Pets[i] = newPet;
                    break;
                }
            }
        }

        /// <summary>
        /// Remove a Pet from the player's team.
        /// </summary>
        /// <param name="pet"></param>
        /// <exception cref="Exception"></exception>
        public void RemovePet(Pet pet)
        {
            if (!Pets.Contains(pet))
                throw new Exception("The Pet was not found.");
            Pets[Array.IndexOf(Pets, pet)] = null;
        }

        /// <summary>
        /// Replace a Pet from the player's team with another one.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newPet"></param>
        public void Replace(Pet old, Pet newPet)
        {
            RemovePet(old);
            AddPet(newPet);
        }

        /// <summary>
        /// Add a consumable to the inventory.
        /// </summary>
        /// <param name="consumable"></param>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddConsumable(Consumable consumable, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than 0.");
            if (Inventory.ContainsKey(consumable))
                Inventory[consumable] += amount;
            else
                Inventory.Add(consumable, amount);
        }

        /// <summary>
        /// Remove an consumable from the inventory.
        /// </summary>
        /// <param name="consumable"></param>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveConsumable(Consumable consumable, int amount = 1)
        {
            if (!Inventory.ContainsKey(consumable))
                throw new Exception("The player do not have this Consumable in his inventory.");
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than 0.");
            Inventory[consumable] -= amount;
            if (Inventory[consumable] < 0)
                throw new Exception("The player have less items than the amount.");
            if (Inventory[consumable] == 0)
                Inventory.Remove(consumable);
        }

        /// <summary>
        /// Create a menu to interact with the inventory.
        /// </summary>
        /// <returns>The menu created</returns>
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
