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
        private Dictionary<PetStat, Alterable<float>> _stats;
        private Dictionary<PetStat, (int t0, int t1, int t2, int t3)> _statsIncrements;
        public float this[PetStat stat]
        {
            get => _stats[stat].AlteratedValue;
            set
            {
                if (value < 0)
                {
                    _stats[stat].Value = 0;
                }
                else
                {
                    _stats[stat].Value = value;
                }
            }
        }
        public bool IsAlive => this[PetStat.Health] > 0;

        /// <summary>
        /// Reset all stats to their base value.
        /// </summary>
        public void ResetStats() => ResetStats((PetStat[])Enum.GetValues(typeof(PetStat)));

        /// <summary>
        /// Reset given stats to their base value.
        /// </summary>
        /// <param name="stats">Stats to reset</param>
        public void ResetStats(PetStat[] stats)
        {
            foreach (PetStat stat in stats)
            {
                _stats[stat].ResetAlterations();
            }
        }

        // Experience
        private int _xp = 0;
        private int _xpForLvlUp = 10;
        public int Xp
        {
            get { return _xp; }
            set
            {
                if (IsMaxLevel)
                    throw new Exception("Already Level Max!");
                if (value < _xp)
                    throw new Exception("A Pet can't lose experience!");
                GainXp(value - _xp);
            }
        }

        private int _level = 1;
        public static int MaxLevel { get; } = 100;
        public bool IsMaxLevel => _level == MaxLevel;
        public static (int t1, int t2, int t3) TierLevels { get; } = (MaxLevel / 10, MaxLevel / 4, MaxLevel / 2);
        public int Tier
        {
            get
            {
                if (_level < TierLevels.t1)
                    return 0;
                if (_level < TierLevels.t2)
                    return 1;
                if (_level < TierLevels.t3)
                    return 2;
                return 3;
            }
        }
        public int Level
        {
            get { return _level; }
            set
            {
                if (IsMaxLevel)
                    throw new Exception("Already Level Max!");
                if (value < _level)
                    throw new Exception("A Pet can't lose a level!");
                LevelUp(value - _level);
            }
        }

        /// <summary>
        /// Gain experience points.
        /// </summary>
        /// <param name="amount"></param>
        public void GainXp(int amount)
        {
            if (IsMaxLevel)
                throw new Exception("Already Level Max!");
            _xp += amount;
            while (_xp > _xpForLvlUp)
            {
                _xp -= _xpForLvlUp;
                LevelUp();
                _xpForLvlUp += _level * 10;
            }
        }

        /// <summary>
        /// Increment the base stats.
        /// </summary>
        public void LevelUp()
        {
            if (IsMaxLevel)
                throw new Exception("Already Level Max!");
            foreach (PetStat stat in (PetStat[])Enum.GetValues(typeof(PetStat)))
            {
                if (_level < TierLevels.t1)
                    _stats[stat].Value += _statsIncrements[stat].t0;
                else if (_level < TierLevels.t2)
                    _stats[stat].Value += _statsIncrements[stat].t1;
                else if (_level < TierLevels.t3)
                    _stats[stat].Value += _statsIncrements[stat].t2;
                else
                    _stats[stat].Value += _statsIncrements[stat].t3;
            }
            _level++;
            ResetStats();
        }

        /// <summary>
        /// Level up x times.
        /// </summary>
        /// <param name="times"></param>
        public void LevelUp(int times)
        {
            if (times < 0)
                throw new Exception("A Pet can't lose a level!");
            for (int i = 0; i < times; i++)
            {
                LevelUp();
            }
        }

        // Abilities
        private Ability[] _abilities = new Ability[4] { Data.Bite, Data.NullAbility, Data.NullAbility, Data.NullAbility };
        public Ability this[int index] { get { return _abilities[index]; } }
        public int AbilityNumber { get { return _abilities.Length; } }

        /// <summary>
        /// Learn an ability.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="index">Slot</param>
        public void LearnAbility(Ability ability, int index)
        {
            if (_abilities.Contains(ability))
                throw new Exception("Pet already knew this ability!");
            if (index > Tier)
                throw new Exception("Pet can't have " + (index + 1).ToString() + " abilities while beeing tier " + Tier.ToString() + "!");
            _abilities[index] = ability;
        }

        /// <summary>
        /// Use an ability on another Pet.
        /// </summary>
        /// <param name="index">Index of the ability</param>
        /// <param name="target">The Pet</param>
        public void UseAbility(int index, Pet target) => _abilities[index].UseOn(target, this);

        /// <summary>
        /// Use an ability on other Pets.
        /// </summary>
        /// <param name="index">Index of the ability</param>
        /// <param name="targets">Pets</param>
        public void UseAbility(int index, Pet[] targets) => _abilities[index].UseOn(targets, this);

        // Status effects
        private List<StatusEffect> _statusEffects = new List<StatusEffect>();
        public StatusEffect[] StatusEffects => _statusEffects.ToArray();

        /// <summary>
        /// Add a status effect.
        /// </summary>
        /// <param name="effect"></param>
        public void AddStatusEffect(StatusEffect effect) => _statusEffects.Add(effect);

        /// <summary>
        /// Remove a status effect.
        /// </summary>
        /// <param name="effect"></param>
        public void RemoveStatusEffect(StatusEffect effect) => _statusEffects.Remove(effect);

        /// <summary>
        /// Clear all status effects.
        /// </summary>
        public void ClearStatusEffects() => _statusEffects.Clear();

        /// <summary>
        /// Update all status effects.
        /// Called at the start of the pet's turn.
        /// </summary>
        public void UpdateStatusEffects()
        {
            for (int i = 0; i < _statusEffects.Count; i++)
            {
                _statusEffects[i].Update(this);
            }
        }


        /// <summary>
        /// Create a new pet.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="type">Type</param>
        /// <param name="stats">Statistics</param>
        /// <param name="statsIncrements">Stats multipliers on level up</param>
        public Pet(string name, PetType type, Dictionary<PetStat, int> stats, Dictionary<PetStat, (int, int, int, int)> statsIncrements)
        {
            Name = name;
            Type = type;
            _stats = new Dictionary<PetStat, Alterable<float>>();
            foreach (PetStat stat in (PetStat[])Enum.GetValues(typeof(PetStat)))
            {
                _stats.Add(stat, new Alterable<float>(stats[stat]));
            }
            _statsIncrements = statsIncrements;
        }
    }
}