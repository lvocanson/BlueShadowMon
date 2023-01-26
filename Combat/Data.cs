namespace BlueShadowMon
{
    public static class Data
    {
        // Abilities

        public static Ability NullAbility { get; } = new Ability("Null BAility", EffectType.Heal, EffectTarget.Self, (Pet target, Pet user) => { });

        public static Ability Bite { get; } = new Ability("Bite", EffectType.PhysicalDamage, EffectTarget.Enemy, (Pet target, Pet user) =>
        {
            target[PetStat.Health] -= user[PetStat.PhysicalDamage] * (1 - (target[PetStat.PhysicalArmor] / (target[PetStat.PhysicalArmor] + 100)));
        });


        // Consumables | Reminder: Consumables's target can't be "Self"

        public static (Consumable I, Consumable II, Consumable III) HealthPotion { get; } =
            (new Consumable("Health Potion I", EffectType.Heal, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.Health] += 20;
            }),
            new Consumable("Health Potion II", EffectType.Heal, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.Health] += 50;
            }),
            new Consumable("Health Potion III", EffectType.Heal, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.Health] += 150;
            }));

        public static Consumable BattlePotion { get; } =
            (new Consumable("Battle Potion", EffectType.PhysicalDamage & EffectType.MagicalDamage, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.PhysicalDamage] += 35;
                target[PetStat.MagicalDamage] += 35;
            }));

        public static Consumable DefensePotion { get; } =
            (new Consumable("Defense Potion", EffectType.Buff, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.PhysicalArmor] += 35;
                target[PetStat.MagicalArmor] += 35;
            }));

        public static Consumable EyeOfTheHerald { get; } =
            (new Consumable("Eye of the Herald", EffectType.PhysicalDamage & EffectType.MagicalDamage, EffectTarget.Ally, (Pet target) =>
            {
                target[PetStat.PhysicalDamage] += target[PetStat.PhysicalDamage] * 2;
                target[PetStat.MagicalDamage] += target[PetStat.MagicalDamage] * 2;
            }));
        public static Consumable RedBull { get; } =
            (new Consumable("RedBull", EffectType.Buff, EffectTarget.Ally, (Pet target) =>
            {
                //One more action for the next 2/3 rounds
            }));

        public static Consumable PetCage { get; } =
            (new Consumable("Pet Cage", EffectType.Status, EffectTarget.Enemy, (Pet[] team) =>
            {
                //Capture the enemy pet
            }));

        // Pets stats

        public static Dictionary<PetStat, int> StarterStats { get; } = new Dictionary<PetStat, int>()
        {
            { PetStat.Health, 100 },
            { PetStat.PhysicalDamage, 10 },
            { PetStat.MagicalDamage, 10 },
            { PetStat.PhysicalArmor, 5 },
            { PetStat.MagicalArmor, 5 }
        };
        public static Dictionary<PetStat, (int, int, int, int)> StarterIncrements { get; } = new Dictionary<PetStat, (int, int, int, int)>()
        {
            { PetStat.Health, (10, 15, 20, 30) },
            { PetStat.PhysicalDamage, (7, 10, 15, 20) },
            { PetStat.MagicalDamage, (7, 10, 15, 20) },
            { PetStat.PhysicalArmor, (0, 1, 3, 5) },
            { PetStat.MagicalArmor, (0, 1, 3, 5) }
        };
    }
}
