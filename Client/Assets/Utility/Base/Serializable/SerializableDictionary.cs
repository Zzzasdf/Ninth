namespace System.Collections.Generic
{
    public class SerializableDictionary<TKey, TValue>: IEnumerable<KeyValuePair<string, TValue>>
    {
        public Dictionary<string, TValue> Container = new();

        public TValue this[TKey key]
        {
            get => Container[key.ToString()];
            set => Container[key.ToString()] = value;
        }
        
        public void Add(TKey key, TValue value)
        {
            Container.Add(key.ToString(), value);
        }

        public bool ContainKey(TKey key)
        {
            return Container.ContainsKey(key.ToString());
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return Container.TryGetValue(key.ToString(), out value);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return Container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}