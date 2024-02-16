using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Ninth
{
    public class TypeSubscribe<TKey, TValue>
    {
        private readonly ReadOnlyDictionary<Type, TValue?> container = new(new Dictionary<Type, TValue?>());

        private TValue? Get(Type type)
        {
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default;
            }
            return result;
        } 
        private void Subscribe(Type type, TValue? value)
        {
            if (!container.TryAdd(type, value))
            {
                $"重复订阅 {type}".FrameError();
            }
        }
        
        public TypeSubscribe<TKey, TValue> Subscribe<T>(TValue? value) where T: TKey
        {
            var type = typeof(T);
            if (!container.TryAdd(type, value))
            {
                $"重复订阅 {nameof(T)}: {type}".FrameError();
            }
            Subscribe(type, value);
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

        public void Add(TKey key, TValue? value)
        {
            Subscribe(key, value);
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

        public bool ContainsKey(TKey key)
        {
            return container.ContainsKey(key);
        }
    }

}