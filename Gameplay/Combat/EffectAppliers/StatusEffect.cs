namespace BlueShadowMon
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

        /// <summary>
        /// Create a new status effect.
        /// </summary>
        /// <param name="name">Name of the status effect</param>
        /// <param name="type">Type of the status effect</param>
        /// <param name="holder">The Pet who hold the status effect</param>
        /// <param name="effectActive">Action executed at each update</param>
        /// <param name="effectEnd">Action executed when theire is no remaining rounds for the effect</param>
        /// <param name="rounds">Number of rounds before the status effect ends</param>
        /// <exception cref="ArgumentException"></exception>
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
