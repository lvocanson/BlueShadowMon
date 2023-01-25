﻿namespace BlueShadowMon
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
        private Dictionary<PetStat, int> _baseStats;
        private Dictionary<PetStat, float> _currentStats = new Dictionary<PetStat, float>();
        private Dictionary<PetStat, (int t0, int t1, int t2, int t3)> _statsIncrements;
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
                _currentStats[stat] = _baseStats[stat];
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
                    _baseStats[stat] += _statsIncrements[stat].t0;
                else if (_level < TierLevels.t2)
                    _baseStats[stat] += _statsIncrements[stat].t1;
                else if (_level < TierLevels.t3)
                    _baseStats[stat] += _statsIncrements[stat].t2;
                else
                    _baseStats[stat] += _statsIncrements[stat].t3;
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

        public void LearnAbility(Ability ability, int index)
        {
            if (index > Tier)
                throw new Exception("Pet can't have " + (index + 1).ToString() + " abilities while beeing tier " + Tier.ToString() + "!");
            _abilities[index] = ability;
        }

        /// <summary>
        /// Use an ability on another Pet.
        /// </summary>
        /// <param name="index">Index of the ability</param>
        /// <param name="target">The Pet</param>
        public void UseAbility(int index, Pet target)
        {
            _abilities[index].UseOn(target, this);
        }

        /// <summary>
        /// Use an ability on other Pets.
        /// </summary>
        /// <param name="index">Index of the ability</param>
        /// <param name="targets">Pets</param>
        public void UseAbility(int index, Pet[] targets)
        {
            _abilities[index].UseOn(targets, this);
        }

        // Status effects
        // TODO: Lucas


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
            _baseStats = stats;
            ResetStats(); //_currentStats
            _statsIncrements = statsIncrements;
        }
    }
}