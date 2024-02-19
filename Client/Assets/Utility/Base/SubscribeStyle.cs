using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Ninth
{
    // 尽量只用于枚举，类型请用泛型订阅
    public class TypeSubscribe<TValue>
    {
        private readonly ReadOnlyDictionary<Type, TValue?> container = new(new Dictionary<Type, TValue?>());
        
        public TValue? this[Type type]
        {
            get => Get(type);
            set => Subscribe(type, value);
        }
        
        public TypeSubscribe<TValue> Subscribe(Type type, TValue? value)
        {
            if (!container.TryAdd(type, value))
            {
                $"重复订阅 {type}".FrameError();
            }
            return this;
        }
        
        private TValue? Get(Type type)
        {
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default;
            }
            return result;
        } 
        
        public ReadOnlyDictionary<Type, TValue?>.KeyCollection Keys => container.Keys;
        public ReadOnlyDictionary<Type, TValue?>.ValueCollection Values => container.Values;

        public bool ContainsKey<T>()
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    // 不能订阅枚举，且不支持索引器
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

        public bool ContainsKey<T>()  where T: TKey
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    public class BaseSubscribe<TKey, TValue>
    {
        private readonly ReadOnlyDictionary<TKey, TValue?> container = new(new Dictionary<TKey, TValue?>());

        public TValue? this[TKey key]
        {
            get => Get(key);
            set => Subscribe(key, value);
        }

        public BaseSubscribe<TKey, TValue> Subscribe(TKey key, TValue? value)
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