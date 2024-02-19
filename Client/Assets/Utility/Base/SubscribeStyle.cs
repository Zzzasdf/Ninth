using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public class GenericsSubscribe<TKey, TValue>
    {
        private readonly ReadOnlyDictionary<Type, TValue?> container = new(new Dictionary<Type, TValue?>());
        
        public GenericsSubscribe<TKey, TValue> Subscribe<T>(TValue? value) where T: TKey
        {
            var type = typeof(T);
            if (!container.TryAdd(type, value))
            {
                $"重复订阅 {nameof(T)}: {type}".FrameError();
            }
            return this;
        }
        
        public TValue? Get<T>() where T: TKey 
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {nameof(T)}: {type}".FrameError();
                return default;
            }
            return result;
        }
        
        public ReadOnlyDictionary<Type, TValue?>.KeyCollection Keys => container.Keys;
        public ReadOnlyDictionary<Type, TValue?>.ValueCollection Values => container.Values;

        public bool ContainsKey<T>()  where T: class, TKey
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    public class EnumTypeSubscribe<TValue>
    {
        private readonly ReadOnlyDictionary<Type, TValue?> container = new(new Dictionary<Type, TValue?>());
        
        public EnumTypeSubscribe<TValue> Subscribe<T>(TValue? value) where T: Enum
        {
            var type = typeof(T);
            if (!container.TryAdd(type, value))
            {
                $"重复订阅 {type}".FrameError();
            }
            return this;
        }
        
        public TValue? Get<T>() where T: Enum
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default;
            }
            return result;
        } 
        
        public ReadOnlyDictionary<Type, TValue?>.KeyCollection Keys => container.Keys;
        public ReadOnlyDictionary<Type, TValue?>.ValueCollection Values => container.Values;

        public bool ContainsKey<T>() where T: Enum
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    public class CommonSubscribe<TKey, TValue>
    {
        private readonly ReadOnlyDictionary<TKey, TValue?> container = new(new Dictionary<TKey, TValue?>());

        public TValue? this[TKey key]
        {
            get => Get(key);
            set => Subscribe(key, value);
        }

        public CommonSubscribe<TKey, TValue> Subscribe(TKey key, TValue? value)
        {
            if (!container.TryAdd(key, value))
            {
                $"重复订阅 {nameof(TKey)}: {key}".FrameError();
            }
            return this;
        }

        public TValue? Get(TKey key)
        {
            if (!container.TryGetValue(key, out var result))
            {
                $"未订阅 {nameof(TKey)}: {key}".FrameError();
                return default;
            }
            return result;
        }

        public ReadOnlyDictionary<TKey, TValue?>.KeyCollection Keys => container.Keys;
        public ReadOnlyDictionary<TKey, TValue?>.ValueCollection Values => container.Values;

        public bool ContainsKey(TKey key)
        {
            return container.ContainsKey(key);
        }
    }
}