using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
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


        public static Consumable MdManaPotion { get; } = new Consumable("Medium Health Potion", EffectType.Heal, EffectTarget.Ally, (Pet target) =>
        {
            target[PetStat.Health] += 50;
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


        // Menus

        public static Dictionary<string, Menu> Menus { get; } = new Dictionary<string, Menu>()
        {
            {
                "Main Menu", new Menu(new Window.ColoredString(Game.GameTitle, ConsoleColor.Blue, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Play"), () => Game.SwitchToMapScene()),
                (new Window.ColoredString("Combat Test"), () => { Console.Beep(); }), // REMOVE THIS LATER
                (new Window.ColoredString("Settings"), () => Game.SwitchToMenuScene("Settings")),
                (new Window.ColoredString("Exit Game"), () => Window.Quit())
            }) },
            {
                "Settings", new Menu(new Window.ColoredString("Settings", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Change Frame Rate"), () => Game.SwitchToMenuScene("Frame Rate")),
                (new Window.ColoredString("Change Window Size"), () => Game.SwitchToMenuScene("Window Size")),
                (new Window.ColoredString("Back"), () => Game.SwitchToMenuScene("Main Menu")),
            }) },
            {
                "Frame Rate", new Menu(new Window.ColoredString("Frame Rate", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("30"), () => { Game.FrameRate = 30; Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("60"), () => { Game.FrameRate = 60; Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("120"), () => { Game.FrameRate = 120; Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("144"), () => { Game.FrameRate = 240; Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("240"), () => { Game.FrameRate = 240; Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("Unlimited"), () => { Game.FrameRate = int.MaxValue; Game.SwitchToMenuScene("Settings"); })
            }, 5) },
            {
                "Window Size", new Menu(new Window.ColoredString("Window Size", ConsoleColor.DarkYellow, Window.DefaultBgColor),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("50%"), () => { Window.Resize(0.5F); Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("60%"), () => { Window.Resize(0.6F); Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("70%"), () => { Window.Resize(0.7F); Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("80%"), () => { Window.Resize(0.8F); Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("90%"), () => { Window.Resize(0.9F); Game.SwitchToMenuScene("Settings"); }),
                (new Window.ColoredString("100%"), () => { Window.Resize(1); Game.SwitchToMenuScene("Settings"); })
            }, 2) }
        };
    }
}
