namespace BlueShadowMon.Gameplay.Combat.EffectAppliers
{
    /// <summary>
    /// A status effect that can be applied to a pet.
    /// Updated at the start of the pet's turns.
    /// </summary>
    public class StatusEffect : EffectApplier
    {
        protected Action _effectActive;
        protected Action _effectEnd;
        protected Pet _holder;
        public int RemainingRounds { get; set; }

        public StatusEffect(Window.ColoredString name, EffectType type, Pet holder, Action effectActive, Action effectEnd, int rounds) : base(name, type, EffectTarget.Self)
        {
            _holder = holder;
            _effectActive = effectActive;
            _effectEnd = effectEnd;
            if (rounds <= 0)
                throw new ArgumentException("Rounds must be greater than 0!");
            RemainingRounds = rounds;
        }

        /// <summary>
        /// Update the status effect on the target.
        /// </summary>
        public void Update()
        {
            RemainingRounds--;
            if (RemainingRounds <= 0)
            {
                _effectEnd();
                _holder.RemoveStatusEffect(this);
            }
            else
            {
                _effectActive();
            }
        }
    }
}
