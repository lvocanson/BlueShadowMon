﻿namespace BlueShadowMon
{
    public static class Data
    {
        /// <summary>
        /// Get ability from id.
        /// </summary>
        public static Dictionary<int, Ability> GetAbilityById { get; } = new();

        /// <summary>
        /// Get consumable from id.
        /// </summary>
        public static Dictionary<int, Consumable> GetConsumableById { get; } = new();

        static Data()
        {
            for (int i = 0; i < Abilities.Length; i++)
            {
                GetAbilityById.Add(i, Abilities[i]);
            }
            for (int i = 0; i < Consumables.Length; i++)
            {
                GetConsumableById.Add(i, Consumables[i]);
            }
        }

        // Abilities
        private static Ability[] Abilities { get; } = new[] {
            new Ability(new Window.ColoredString("Null Ability"), EffectType.Heal, EffectTarget.Self, (targets, user) =>
            {
                ;
            }),

            /// <summary>
            /// A physical attack that deals damage to a single target.
            /// </summary>
            new Ability(new Window.ColoredString("Attack"), EffectType.Damage, EffectTarget.Enemy, (targets, user) =>
            {
                Pet target = targets[0]; // Only one target
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return health - Math.Max(user[PetStat.Power] - target[PetStat.Armor], 1);
                });
            }),

            /// <summary>
            /// Heal the targets for 50% of the user's power (splited between the tarets).
            /// </summary>
            new Ability(new Window.ColoredString("Heal"), EffectType.Heal, EffectTarget.Multiple | EffectTarget.Ally, (targets, user) =>
            {
                foreach(Pet target in targets)
                {
                    target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                    {
                        return Math.Min(health + user[PetStat.Power] * 0.5F / targets.Length, target.BaseStats[PetStat.Health]);
                    });
                }
            }),

            /// <summary>
            /// Buff the user's power by 25% for 3 turns.
            /// </summary>
            new Ability(new Window.ColoredString("Power Buff"), EffectType.Buff, EffectTarget.Self, (targets, user) =>
            {
                Pet target = targets[0]; // Only one target
                AlterationID aid = target.AlterStat(PetStat.Power, AlterationType.Multiplicative, (power) =>
                {
                    return power * 1.25F;
                });
                target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Power Buff"), EffectType.Buff, target, () => { }, () =>
                {
                    target.RemoveStatAlteration(PetStat.Power, aid);
                }, 3));
            }),

            /// <summary>
            /// Debuff an enemy's power by 25% for 3 turns.
            /// </summary>
            new Ability(new Window.ColoredString("Power Debuff"), EffectType.Debuff, EffectTarget.Enemy, (targets, user) =>
            {
                Pet target = targets[0]; // Only one target
                AlterationID aid = target.AlterStat(PetStat.Power, AlterationType.Multiplicative, (power) =>
                {
                    return power * 0.75F;
                });
                target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Power Debuff"), EffectType.Buff, target, () => { }, () =>
                {
                    target.RemoveStatAlteration(PetStat.Power, aid);
                }, 3));
            })
        };

        /// Consumables | Reminder: Consumables's target can't be "Self"

        // Healing Consumables

        private static Consumable[] Consumables { get; } =
        {
            new Consumable(new Window.ColoredString("Health Potion I"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return Math.Min(health + 20,  target.BaseStats[PetStat.Health]);
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion II"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return Math.Min(health + 70,  target.BaseStats[PetStat.Health]);
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion III"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
                {
                    return Math.Min(health + 150,  target.BaseStats[PetStat.Health]);
                });
            })
        };


        // Pets stats

        public static Dictionary<PetStat, float> StarterStats { get; } = new()
        {
            { PetStat.Health, 50 },
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
