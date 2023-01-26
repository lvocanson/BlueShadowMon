namespace BlueShadowMon
{
    /// <summary>
    /// A status effect that can be applied to a pet.
    /// Updated at the start of the pet's turns.
    /// </summary>
    public class StatusEffect : EffectApplier
    {
        protected Action<Pet> _effectActive;
        protected Action<Pet> _effectEnd;
        public int RemainingRounds { get; set; }

        public StatusEffect(string name, EffectType type, Action<Pet> effectActive, Action<Pet> effectEnd, int rounds) : base(name, type, EffectTarget.Self)
        {
            _effectActive = effectActive;
            _effectEnd = effectEnd;
            if (rounds <= 0)
                throw new ArgumentException("Rounds must be greater than 0!");
            RemainingRounds = rounds;
        }

        /// <summary>
        /// Update the status effect on the target.
        /// </summary>
        /// <param name="target"></param>
        public void Update(Pet target)
        {
            RemainingRounds--;
            if (RemainingRounds <= 0)
            {
                _effectEnd(target);
                target.RemoveStatusEffect(this);
            }
            else
            {
                _effectActive(target);
            }
        }
    }
}
