namespace BlueShadowMon
{
    public enum EffectType
    {
        PhysicalDamage = 1,
        MagicalDamage = 2,
        Heal = 4,
        Buff = 8,
        Debuff = 16,
        StatusEffect = 32,
    }

    public enum EffectTarget
    {
        Self = 1,
        Ally = 2,
        Enemy = 4,
        Multiple = 8,
    }

    public abstract class EffectApplier
    {
        public Window.ColoredString Name { get; }
        public EffectType Type { get; }
        public EffectTarget Target { get; }
        public bool CanTarget(EffectTarget wanted)
        {
            return (Target & wanted) == wanted;
        }

        public EffectApplier(Window.ColoredString name, EffectType type, EffectTarget target)
        {
            Name = name;
            Type = type;
            Target = target;
        }
    }
}
