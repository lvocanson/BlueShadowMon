
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

        public void AddConsumable(Consumable consumable, int amount)
        {
            Inventory.Add(consumable, amount);
        }

        public void RemovePet(Pet pet)
        {
            if (Pets.Contains(pet))
            {
                Pets.Remove(pet);
            }
        }

        public void RemoveConsumable(Consumable consumable)
        {
            if (Inventory[consumable].Equals(0))
            {
                Inventory.Remove(consumable);
            }
        }

    }
}
