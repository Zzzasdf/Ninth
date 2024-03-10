using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Collections.Generic
{
    public class SerializableDictionary<TKey, TValue>
        where TKey: struct
    {
        public Dictionary<string, TValue> container = new();
 
        public void Add(TKey key, TValue value)
        {
            container.Add(key.ToString(), value);
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return container.TryGetValue(key.ToString(), out value);
        }
    }
}