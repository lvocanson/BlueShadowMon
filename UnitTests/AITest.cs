namespace UnitTests
{
    public class AITest
    {
        public Dictionary<PetStat, float> lowHealthPetStats()
        {
            Dictionary<PetStat, float> stats = new (Data.StarterStats);
            stats[PetStat.Health] = 1;
            return stats;
        }

        [Test]
        public void TestAiWhenAnEnemyIsLowOnHealth()
        {
            List<Pet> Allies = new()
                {
                new Pet("Dummy Dog", PetType.Dog, Data.StarterStats, Data.StarterIncrements),
                new Pet("Dummy Cat", PetType.Cat, lowHealthPetStats(), Data.StarterIncrements),
            };
            List<Pet> Enemies = new()
                {
                new Pet("AI Snake", PetType.Snake, Data.StarterStats, Data.StarterIncrements),
                new Pet("AI Dog", PetType.Dog, Data.StarterStats, Data.StarterIncrements),
            };
            Pet ActivePet = Enemies[0];
            AI.Play(ActivePet, Allies, Enemies);

            Assert.That(Allies[1].IsAlive, Is.EqualTo(false));
        }
    }
}