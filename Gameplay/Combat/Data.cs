namespace BlueShadowMon
{
    public static class Data
    {
        // Abilities

        public static Ability NullAbility { get; } = new Ability(new Window.ColoredString("Null Ability"), EffectType.Heal, EffectTarget.Self, (target, user) =>
        {
            ;
        });


        // Physicial damage Abilities with or without buff or debuff or healing

        public static Ability Bite { get; } = new Ability(new Window.ColoredString("Bite"), EffectType.PhysicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, AlterationType.Additive, (health) =>
            {
                return health - 10;
            });
        });

        public static Ability NutShot { get; } = new Ability(new Window.ColoredString("Nut Shot"), EffectType.PhysicalDamage | EffectType.Heal, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 30;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Nut Shot"), EffectType.PhysicalDamage | EffectType.Heal, target, () =>
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

        public static Ability Charge { get; } = new Ability(new Window.ColoredString("Charge"), EffectType.PhysicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 20;
            });
        });

        public static Ability Kick { get; } = new Ability(new Window.ColoredString("Kick"), EffectType.PhysicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 15;
            });
        });

        public static Ability Headbutt { get; } = new Ability(new Window.ColoredString("Headbutt"), EffectType.PhysicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 25;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Headbutt"), EffectType.PhysicalDamage, target, () =>
            {
            }, () =>
            {
                // TODO : Apply stun effect
            }, 1));
        });

        // Magical damage Abilities with or without buff or debuff or healing

        public static Ability ShyningStar { get; } = new Ability(new Window.ColoredString("Shyning Star"), EffectType.MagicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 10;
            });
        });

        public static Ability Vampirism { get; } = new Ability(new Window.ColoredString("Vampirism"), EffectType.MagicalDamage | EffectType.Heal, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 20;
            });
            user.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health + 10;
            });
        });

        public static Ability IceShard { get; } = new Ability(new Window.ColoredString("Ice Shard"), EffectType.MagicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 15;
            });
        });

        public static Ability FireBall { get; } = new Ability(new Window.ColoredString("Fire Ball"), EffectType.MagicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 20;
            });
        });

        public static Ability MagicalCage { get; } = new Ability(new Window.ColoredString("Magical Cage"), EffectType.MagicalDamage, EffectTarget.Enemy, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health - 25;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Magical Cage"), EffectType.MagicalDamage, target, () =>
            {
            }, () =>
            {
                // TODO : Apply stun effect
            }, 1));
        });


        // Healing abilities

        public static Ability BiscuitRain { get; } = new Ability(new Window.ColoredString("Biscuit Rain"), EffectType.Heal, EffectTarget.Self, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health + 10;
            });
        });

        public static Ability Meditation { get; } = new Ability(new Window.ColoredString("Meditation"), EffectType.Heal, EffectTarget.Self, (target, user) =>
        {
            target.AlterStat(PetStat.Health, 0, (health) =>
            {
                return health + 30;
            });
            // TODO : User can't attack for 1 turn
        });


        // Buffs Abilities

        public static Ability AttackBuff { get; } = new Ability(new Window.ColoredString("Attack Buff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalDamage, AlterationType.Additive, (pDamage) =>
            {
                return pDamage + 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Attack Buff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalDamage, aid);
            }, 3));
        });

        public static Ability DefenseBuff { get; } = new Ability(new Window.ColoredString("Defense Buff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalArmor, AlterationType.Additive, (pDefense) =>
            {
                return pDefense + 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Defense Buff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalArmor, aid);
            }, 3));
        });

        public static Ability MagicalAttackBuff { get; } = new Ability(new Window.ColoredString("Magical Attack Buff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.MagicalDamage, AlterationType.Additive, (mDamage) =>
            {
                return mDamage + 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Magical Attack Buff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.MagicalDamage, aid);
            }, 3));
        });

        public static Ability MagicalDefenseBuff { get; } = new Ability(new Window.ColoredString("Magical Defense Buff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.MagicalArmor, AlterationType.Additive, (mDefense) =>
            {
                return mDefense + 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Magical Defense Buff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.MagicalArmor, aid);
            }, 3));
        });


        // Debuffs Abilities

        public static Ability AttackDebuff { get; } = new Ability(new Window.ColoredString("Attack Debuff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalDamage, AlterationType.Additive, (pDamage) =>
            {
                return pDamage - 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Attack Debuff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalDamage, aid);
            }, 3));
        });

        public static Ability DefenseDebuff { get; } = new Ability(new Window.ColoredString("Defense Debuff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.PhysicalArmor, AlterationType.Additive, (pDefense) =>
            {
                return pDefense - 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Defense Debuff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.PhysicalArmor, aid);
            }, 3));
        });

        public static Ability MagicalAttackDebuff { get; } = new Ability(new Window.ColoredString("Magical Attack Debuff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.MagicalDamage, AlterationType.Additive, (mDamage) =>
            {
                return mDamage - 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Magical Attack Debuff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.MagicalDamage, aid);
            }, 3));
        });

        public static Ability MagicalDefenseDebuff { get; } = new Ability(new Window.ColoredString("Magical Defense Debuff"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            AlterationID aid = target.AlterStat(PetStat.MagicalArmor, AlterationType.Additive, (mDefense) =>
            {
                return mDefense - 10;
            });
            target.AddStatusEffect(new StatusEffect(new Window.ColoredString("Magical Defense Debuff"), EffectType.Buff, target, () => { }, () =>
            {
                target.RemoveStatAlteration(PetStat.MagicalArmor, aid);
            }, 3));
        });

        // Apply stun 
        public static Ability Stun { get; } = new Ability(new Window.ColoredString("Stun"), EffectType.Buff, EffectTarget.Self, (target, user) =>
        {
            // TODO
        });


        /// Consumables | Reminder: Consumables's target can't be "Self"

        // Healing Consumables

        public static (Consumable I, Consumable II, Consumable III) HealthPotion { get; } =
            (new Consumable(new Window.ColoredString("Health Potion I"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 20;
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion II"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 50;
                });
            }),
            new Consumable(new Window.ColoredString("Health Potion III"), EffectType.Heal, EffectTarget.Ally, (target) =>
            {
                target.AlterStat(PetStat.Health, 0, (health) =>
                {
                    return health + 150;
                });
            }));

        // Buffs Consumables

        public static Consumable BattlePotion { get; } =
            new Consumable(new Window.ColoredString("Battle Potion"), EffectType.PhysicalDamage & EffectType.MagicalDamage, EffectTarget.Ally, (target) =>
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
            new Consumable(new Window.ColoredString("Defense Potion"), EffectType.Buff, EffectTarget.Ally, (target) =>
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

        // Revive Consumables
        public static Consumable RevivePotion { get; } =
            new Consumable(new Window.ColoredString("Revive Potion"), EffectType.Buff, EffectTarget.Ally, (target) =>
            {
                if (target.IsAlive == false)
                {
                    target.AlterStat(PetStat.Health, 0, (health) =>
                    {
                        return health + 100;
                    });
                }
            });

        // Capture Consumables
        public static Consumable PetCage { get; } =
            new Consumable(new Window.ColoredString("Pet Cage"), EffectType.Buff, EffectTarget.Ally, (target) =>
            {
                // TODO
            });




        // Pets stats

        public static Dictionary<PetStat, int> StarterStats { get; } = new()
        {
            { PetStat.Health, 100 },
            { PetStat.PhysicalDamage, 10 },
            { PetStat.MagicalDamage, 10 },
            { PetStat.PhysicalArmor, 5 },
            { PetStat.MagicalArmor, 5 }
        };
        public static Dictionary<PetStat, (int, int, int, int)> StarterIncrements { get; } = new()
        {
            { PetStat.Health, (10, 15, 20, 30) },
            { PetStat.PhysicalDamage, (7, 10, 15, 20) },
            { PetStat.MagicalDamage, (7, 10, 15, 20) },
            { PetStat.PhysicalArmor, (0, 1, 3, 5) },
            { PetStat.MagicalArmor, (0, 1, 3, 5) }
        };
    }
}
