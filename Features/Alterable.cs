namespace BlueShadowMon
{
    public class AlterationID { }
    public class Alterable<T>
    {
        public T Value { get; set; }
        public T AlteratedValue { get; private set; }

        private List<(AlterationID id, Func<T, T> alteration, int order)> _alterations = new List<(AlterationID id, Func<T, T> alteration, int order)>();

        public Alterable(T baseValue)
        {
            Value = baseValue;
            AlteratedValue = baseValue;
        }

        public AlterationID Alterate(int order, Func<T, T> alteration)
        {
            AlterationID id = new AlterationID();
            for (int i = 0; i < _alterations.Count; i++)
            {
                if (_alterations[i].order == order)
                {
                    _alterations.Insert(i, (id, alteration, order));
                    return id;
                }
            }
            _alterations.Add((id, alteration, order));
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
