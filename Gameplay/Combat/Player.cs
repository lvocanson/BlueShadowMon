
namespace BlueShadowMon
{
    class Player
    {
        public Dictionary<Consumable, int> Inventory { get; private set; } = new();
        public List<Pet> Pets { get; } = new();
        

        public Player(Dictionary<Consumable, int> inventory, List<Pet> pets)
        {

            
        }

        private void AddPet(Pet newPet)
        {
            Pets.Add(newPet);
        }

        private void AddConsumable(Consumable consumable, int amount)
        {
            Inventory.Add(consumable, amount);
        }

        private void RemovePet(Pet pet)
        {
            if (Pets.Contains(pet))
            {
                Pets.Remove(pet);
            }
        }

        private void RemoveConsumable(Consumable consumable)
        {
            if (Inventory[consumable].Equals(0))
            {
                Inventory.Remove(consumable);
            }
        }

    }
}
