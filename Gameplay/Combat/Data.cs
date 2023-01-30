using BlueShadowMon.Gameplay.Combat.EffectAppliers;

namespace BlueShadowMon.Gameplay.Combat
{
    public static class Data
    {
        // Abilities

        public static Ability NullAbility { get; } = new Ability("Null BAility", EffectType.Heal, EffectTarget.Self, (target, user) => { });


        // Physicial damage Abilities with or without buff or debuff or healing

        public static Ability Bite { get; } = new Ability("Bite", EffectType.PhysicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 10;
            });
        });

        public static Ability NutShot { get; } = new Ability("Nut Shot", EffectType.PhysicalDamage | EffectType.Heal, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 30;
            });
            target.AddStatusEffect(new StatusEffect("Nut Shot", EffectType.PhysicalDamage | EffectType.Heal, target, () =>
            {
            }, () =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 25;
                });
                // TODO : Apply stun effect
            }, 1));

        });

        // Magical damage Abilities with or without buff or debuff or healing

        public static Ability ShyningStar { get; } = new Ability("Shyning Star", EffectType.MagicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 10;
            });
        });


        // Healing abilities

        public static Ability BiscuitRain { get; } = new Ability("Biscuit Rain", EffectType.Heal, EffectTarget.Self, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health + 10;
            });
        });


        // Buffs Abilities

        public static Ability AttackBuff { get; } = new Ability("Attack Buff", EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalDamage, AlterationType.Additive, (pDamage) =>
            {
                return pDamage + 10;
            });
            target.AddStatusEffect(new StatusEffect("Attack Buff", EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalDamage, aid);
            }, 3));
        });


        // Debuffs Abilities

        public static Ability AttackDebuff { get; } = new Ability("Attack Debuff", EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalDamage, AlterationType.Additive, (pDamage) =>
            {
                return pDamage - 10;
            });
            target.AddStatusEffect(new StatusEffect("Attack Debuff", EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalDamage, aid);
            }, 3));
        });


        /// Consumables | Reminder: Consumables's target can't be "Self"

        // Healing Consumables

        public static (Consumable I, Consumable II, Consumable III) HealthPotion { get; } =
            (new Consumable("Health Potion I", EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 20;
                });
            }),
            new Consumable("Health Potion II", EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 50;
                });
            }),
            new Consumable("Health Potion III", EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 150;
                });
            }));

        // Buffs Consumables

        public static Consumable BattlePotion { get; } =
            new Consumable("Battle Potion", EffectType.PhysicalDamage & EffectType.MagicalDamage, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.PhysicalDamage, AlterationType.Additive, (pDamage) =>
                {
                    return pDamage + 10;
                });
                target.AlterStat(PetStat.MagicalDamage, AlterationType.Additive, (mDamage) =>
                {
                    return mDamage + 10;
                });
            });

        public static Consumable DefensePotion { get; } =
        new Consumable("Defense Potion", EffectType.Buff, EffectTarget.Ally, (target) =>
        {
            target.AlterStat(PetStat.PhysicalArmor, AlterationType.Additive, (pDefense) =>
            {
                return pDefense + 10;
            });
            target.AlterStat(PetStat.MagicalArmor, AlterationType.Additive, (mDefense) =>
            {
                return mDefense + 10;
            });
        });


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
