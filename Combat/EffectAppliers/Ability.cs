namespace BlueShadowMon
{
    /// <summary>
    /// An ability that can be used by a pet.
    /// </summary>
    public class Ability : EffectApplier
    {
        protected Action<Pet, Pet> _effect;
        public Ability(string name, EffectType type, EffectTarget target, Action<Pet, Pet> effect) : base(name, type, target)
        {
            _effect = effect;
        }

        /// <summary>
        /// Use the ability on a single target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="user"></param>
        public void UseOn(Pet target, Pet user)
        {
            _effect(target, user);
        }

        /// <summary>
        /// Use the ability on multiple targets
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="user"></param>
        public void UseOn(Pet[] targets, Pet user)
        {
            if (!HasTargetable(EffectTarget.Multiple))
                throw new Exception("This ability cannot be used on multiple targets.");
            foreach (Pet t in targets)
            {
                _effect(t, user);
            }
        }
    }
}

