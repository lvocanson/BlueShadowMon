namespace BlueShadowMon
{
    public enum PetType
    {
        Dog = 0,
        Cat = 1,
        Snake = 2,
    }

    public enum PetStat
    {
        Health = 0,
        PhysicalDamage = 1,
        MagicalDamage = 2,
        PhysicalArmor = 3,
        MagicalArmor = 4,
    }
    
    public class Pet
    {

        public string Name { get; }
        public PetType Type { get; }

        // Stats
        private Dictionary<PetStat, float> _baseStats = new Dictionary<PetStat, float>();
        private Dictionary<PetStat, float> _currentStats = new Dictionary<PetStat, float>();
        public float this[PetStat stat]
        {
            get => _currentStats[stat];
            set
            {
                if (value < 0)
                {
                    _currentStats[stat] = 0;
                }
                else if (value > _baseStats[stat])
                {
                    _currentStats[stat] = _baseStats[stat];
                }
                else
                {
                    _currentStats[stat] = value;
                }
            }
        }
        public bool IsAlive => this[PetStat.Health] > 0;

        // Abilities
        private List<Effect> _abilities = new List<Effect>();
        public Effect this[int index] => _abilities[index];

        public Pet(string name, PetType type, Dictionary<PetStat, int> stats)
        {
            Name = name;
            Type = type;
            foreach (PetStat stat in Enum.GetValues(typeof(PetStat)))
            {
                _baseStats[stat] = stats[stat];
                _currentStats[stat] = stats[stat];
            }
        }
    }
}