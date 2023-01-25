namespace BlueShadowMon
{
    internal class Pet
    {
        private const char HealthBarLeftBracket = '[';
        private const char HealthBarFiller = 'H';
        private const char HealthBarEmpty = '-';
        private const char HealthBarRightBracket = ']';

        public Pet(string name, int level, int health, int attack, int defense)
        {
            Name = name;
            Level = level;
            MaxHealth = health;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public string Name { get; }
        public int Level { get; private set; }
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }

        readonly List<Ability> Abilities = new List<Ability>();

        public void AddAbility(string name, int attack)
        {
           Abilities.Add(new Ability(name, attack));
        }

        public void AttackEnemy(Pet enemy)
        {
            enemy.Health -= Math.Min(1, Attack / enemy.Defense);
        }

        
        public string GetHealthBar(int width = 10)
        {
            int porcent = Health * (width - 2) / MaxHealth;
            return HealthBarLeftBracket + new string(HealthBarFiller, porcent) + new string(HealthBarEmpty, width - 2 - porcent) + HealthBarRightBracket;
        }

        public void ResetHealth() => Health = MaxHealth;

    }

    internal class Ability
    {
        public Ability(string name, int attack)
        {
            Name = name;
            AttackAbility = attack;
        }

        public string Name { get; }
        public int AttackAbility { get; private set; }

    }
}