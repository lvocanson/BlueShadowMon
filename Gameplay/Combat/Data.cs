namespace BlueShadowMon
{
    public static class Data
    {
        // Abilities

        public static Ability NullAbility { get; } = new Ability(new Window.ColoredString("Null Ability"), EffectType.Heal, EffectTarget.Self, (target, user) =>
        {
            ;
        });

        /// <summary>
        /// A physical attack that deals damage to a single target.
        /// </summary>
        public static Ability Attack { get; } = new Ability(new Window.ColoredString("Attack"), EffectType.Damage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
            {
                return health - Math.Max(user[PetStat.Power] - target[PetStat.Armor], 1);
            });
        });

        /// <summary>
        /// Heal the target for 25% of the user's power.
        /// </summary>
        public static Ability Heal { get; } = new Ability(new Window.ColoredString("Heal"), EffectType.Heal, EffectTarget.Multiple | EffectTarget.Ally, (target, user) =>
        {
            target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
            {
                return health + user[PetStat.Power] * 0.25F;
            });
        });

        /// <summary>
        /// Buff the user's power by 25% for 3 turns.
        /// </summary>
        public static Ability PowerBuff { get; } = new Ability(new Window.ColoredString("Power Buff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.Power, AlterationType.Multiplicative, (power) =>
            {
                return power * 0.25F;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Power Buff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.Power, aid);
            }, 3));
        });

        /// <summary>
        /// Buff the user's armor by 25% for 3 turns.
        /// </summary>
        public static Ability PowerDebuff { get; } = new Ability(new Window.ColoredString("Power Debuff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.Power, AlterationType.Multiplicative, (power) =>
            {
                return power * 0.25F;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Power Debuff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.Power, aid);
            }, 3));
        });

        /// Consumables | Reminder: Consumables's target can't be "Self"

        // Healing Consumables

        public static (Consumable I, Consumable II, Consumable III) HealthPotion { get; } =
            (new Consumable(new Window.ColoredString("Health Potion I"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return health + 20;
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion II"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return health + 50;
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion III"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return health + 150;
                });
            }));

        // Buffs Consumables

        public static Consumable BattlePotion { get; } =
            new Consumable(new Window.ColoredString("Battle Potion"), EffectType.Damage, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Power, AlterationType.Additive, (pDamage) =>
                {
                    return pDamage + 10;
                });
            });

        public static Consumable DefensePotion { get; } =
            new Consumable(new Window.ColoredString("Defense Potion"), EffectType.Buff, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Armor, AlterationType.Additive, (pDefense) =>
                {
                    return pDefense + 10;
                });
            });

        // Revive Consumables
        public static Consumable RevivePotion { get; } =
            new Consumable(new Window.ColoredString("Revive Potion"), EffectType.Buff, EffectTarget.Ally, (target) =>
            {
                if (target.IsAlive == false)
                {
                    target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                    {
                        return health + 100;
                    });
                }
            });

        // Capture Consumables : Implement later
        public static Consumable PetCage { get; } =
            new Consumable(new Window.ColoredString("Pet Cage"), EffectType.Buff, EffectTarget.Ally, (target) =>
            {
                // TODO
            });




        // Pets stats

        public static Dictionary<PetStat, int> StarterStats { get; } = new()
        {
            { PetStat.Health, 100 },
            { PetStat.Power, 10 },
            { PetStat.Armor, 5 },
        };
        public static Dictionary<PetStat, (int, int, int, int)> StarterIncrements { get; } = new()
        {
            { PetStat.Health, (10, 15, 20, 30) },
            { PetStat.Power, (7, 10, 15, 20) },
            { PetStat.Armor, (0, 1, 3, 5) },
        };
    }
}
