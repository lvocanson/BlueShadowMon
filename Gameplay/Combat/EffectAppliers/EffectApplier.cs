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

        /// <summary>
        /// Base constructor for EffectApplier.
        /// </summary>
        /// <param name="name">Name of the effect</param>
        /// <param name="type">Type of the efect</param>
        /// <param name="target">Possible targets of the effect</param>
        public EffectApplier(Window.ColoredString name, EffectType type, EffectTarget target)
        {
            Name = name;
            Type = type;
            Target = target;
        }

        /// <summary>
        /// Check if the EffectApplier is a certain EffectType.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>True if it is, false otherwise</returns>
        public bool isEffectType(EffectType type) => (Type & type) == type;

        /// <summary>
        /// Check if the EffectApplier can target a certain EffectTarget.
        /// </summary>
        /// <param name="wanted"></param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanTarget(EffectTarget wanted) => (Target & wanted) == wanted;
    }
}
