namespace BlueShadowMon
{
    public enum AlterationType
    {
        Additive = 1,
        Multiplicative = 2,
    }
    public class AlterationID { }
    public class Alterable<T>
    {
        public T BaseValue { get; set; }
        public T AlteratedValue { get; private set; }

        private List<(AlterationID id, Func<T, T> alteration, AlterationType type)> _alterations = new List<(AlterationID id, Func<T, T> alteration, AlterationType type)>();

        public Alterable(T baseValue)
        {
            BaseValue = baseValue;
            AlteratedValue = baseValue;
        }

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

        public void ResetAlterations()
        {
            _alterations.Clear();
        }
    }
}
