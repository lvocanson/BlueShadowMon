namespace BlueShadowMon
{
    public enum AlterationType
    {
        Additive = 1,
        Multiplicative = 2,
    }
    public class AlterationID { }

    /// <summary>
    /// A value that can be altered by multiple functions.
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class Alterable<T>
    {
        public T BaseValue { get; set; }
        public T AlteratedValue { get; private set; }

        private List<(AlterationID id, Func<T, T> alteration, AlterationType type)> _alterations = new List<(AlterationID id, Func<T, T> alteration, AlterationType type)>();

        /// <summary>
        /// Create a new Alterable value.
        /// </summary>
        /// <param name="baseValue">The base value of the Alterable</param>
        public Alterable(T baseValue)
        {
            BaseValue = baseValue;
            AlteratedValue = baseValue;
        }

        /// <summary>
        /// Add a function to alterate the value.
        /// </summary>
        /// <param name="type">Additive or Multiplicative (Additive are calculated before Multiplicative)</param>
        /// <param name="alteration">The function to apply</param>
        /// <returns>The ID of the alteration</returns>
        public AlterationID Alterate(AlterationType type, Func<T, T> alteration)
        {
            // Add alteration to the list
            AlterationID id = new AlterationID();
            for (int i = 0; i < _alterations.Count; i++)
            {
                if (_alterations[i].type == type)
                {
                    _alterations.Insert(i, (id, alteration, type));
                    break;
                }
            }
            _alterations.Add((id, alteration, type));

            // Update the alterated value
            AlteratedValue = BaseValue;
            foreach (AlterationType t in Enum.GetValues(typeof(AlterationType)))
            {
                for (int i = 0; i < _alterations.Count; i++)
                {
                    if (_alterations[i].type == t)
                    {
                        AlteratedValue = _alterations[i].alteration(AlteratedValue);
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// Remove an alteration from the list.
        /// </summary>
        /// <param name="id">The ID of the alteration to remove</param>
        public void RemoveAlteration(AlterationID id)
        {
            for (int i = 0; i < _alterations.Count; i++)
            {
                if (_alterations[i].id == id)
                {
                    _alterations.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove all alterations.
        /// </summary>
        public void ResetAlterations()
        {
            _alterations.Clear();
            AlteratedValue = BaseValue;
        }
    }
}
