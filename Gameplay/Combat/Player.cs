namespace BlueShadowMon
{
    public class Player
    {
        public const int MAX_PETS = 3;
        public Dictionary<int, int> Inventory { get; private set; }
        public List<Pet> Pets { get; }
        public int x { get; private set; }
        public int y { get; private set; }
        
        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="mapPosition">Position on the map</param>
        public Player(int x, int y)
        {
            Inventory = new Dictionary<int, int>();
            Pets = new()
            {
                new Pet("MyDog", PetType.Dog, Data.StarterStats, Data.StarterIncrements)
            };
            (this.x, this.y) = (x, y);
        }

        /// <summary>
        /// Move the player to the specified x and y.
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void Move(int newX, int newY) => (x, y) = (newX, newY);

        /// <summary>
        /// Move the player to the specified position.
        /// </summary>
        /// <param name="playerPos"></param>
        public void Move((int x, int y) playerPos) => (x, y) = playerPos;

        /// <summary>
        /// Add a Pet to the player's team.
        /// </summary>
        /// <param name="newPet"></param>
        /// <exception cref="Exception"></exception>
        public void AddPet(Pet newPet)
        {
            if (Pets.Count >= MAX_PETS)
                throw new Exception($"Maximum number of Pets ({MAX_PETS}) exceeded.");
            Pets.Add(newPet);
        }

        /// <summary>
        /// Remove a Pet from the player's team.
        /// </summary>
        /// <param name="pet"></param>
        /// <exception cref="Exception"></exception>
        public void RemovePet(Pet pet)
        {
            if (!Pets.Remove(pet))
                throw new Exception("The Pet was not found.");
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
        /// <param name="consumableId"></param>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddConsumable(int consumableId, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than 0.");
            if (Inventory.ContainsKey(consumableId))
                Inventory[consumableId] += amount;
            else
                Inventory.Add(consumableId, amount);
        }

        /// <summary>
        /// Remove an consumable from the inventory.
        /// </summary>
        /// <param name="consumableId"></param>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveConsumable(int consumableId, int amount = 1)
        {
            if (!Inventory.ContainsKey(consumableId))
                throw new Exception("The player do not have this Consumable in his inventory.");
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than 0.");
            Inventory[consumableId] -= amount;
            if (Inventory[consumableId] < 0)
                throw new Exception("The player have less items than the amount.");
            if (Inventory[consumableId] == 0)
                Inventory.Remove(consumableId);
        }

        /// <summary>
        /// Create a menu to interact with the inventory.
        /// </summary>
        /// <returns>The menu created</returns>
        public Menu CreateInventoryMenu()
        {
            List<(Window.ColoredString, Action)> actions = new();

            foreach (int id in Inventory.Keys)
            {
                Consumable c = Data.GetConsumableById[id];
                Window.ColoredString name = c.Name;
                name.String = $"{Inventory[id]} - {name.String}";
                actions.Add((name, () =>
                {
                    // TODO: Use the consumable
                }
                ));
            }

            if (actions.Count == 0)
            {
                actions.Add((new Window.ColoredString("- Empty -"), () => { }));
            }

            return new Menu(new Window.ColoredString("Inventory (Press ESC to close)", ConsoleColor.Yellow, Window.DefaultBgColor), actions.ToArray());
        }
    }
}
