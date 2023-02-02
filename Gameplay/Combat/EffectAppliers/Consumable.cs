namespace BlueShadowMon
{
    /// <summary>
    /// A consumable that can be used by a player.
    /// </summary>
    public class Consumable : EffectApplier
    {
        protected Action<Pet> _effect;

        /// <summary>
        /// Create a new consumable.
        /// </summary>
        /// <param name="name">Name of the consumable</param>
        /// <param name="type">Type of the consumable</param>
        /// <param name="target">Possible targets</param>
        /// <param name="effect">Action to execute when the consumable is used</param>
        /// <exception cref="ArgumentException"></exception>
        public Consumable(Window.ColoredString name, EffectType type, EffectTarget target, Action<Pet> effect) : base(name, type, target)
        {
            if (CanTarget(EffectTarget.Self))
            {
                throw new ArgumentException("A consumable can't have 'Self' as a target!");
            }
            _effect = effect;
        }

        /// <summary>
        /// Use the consumable on a single target.
        /// </summary>
        /// <param name="target"></param>
        public void UseOn(Pet target)
        {
            _effect(target);
        }

        /// <summary>
        /// Use the consumable on multiple targets
        /// </summary>
        /// <param name="targets"></param>
        public void UseOn(Pet[] targets)
        {
            if (!CanTarget(EffectTarget.Multiple))
                throw new Exception("This consumable cannot be used on multiple targets.");
            foreach (Pet t in targets)
            {
                _effect(t);
            }
        }
    }
}
