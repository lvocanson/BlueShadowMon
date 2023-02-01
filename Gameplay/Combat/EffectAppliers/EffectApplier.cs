namespace BlueShadowMon
{
    public enum EffectType
    {
        Damage = 1,
        Heal = 2,
        Buff = 4,
        Debuff = 8,
        StatusEffect = 16,
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
        public bool isEffectType(EffectType type) => (Type & type) == type;
        public bool CanTarget(EffectTarget wanted) => (Target & wanted) == wanted;

        public EffectApplier(Window.ColoredString name, EffectType type, EffectTarget target)
        {
            Name = name;
            Type = type;
            Target = target;
        }
    }
}
