using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OfficeOpenXml.FormulaParsing.Utilities;
using UnityEngine;
using LitJson;

namespace Ninth.Utility
{
    public class GenericsSubscribe<TKey, TValue>
    {
        private readonly Dictionary<Type, ReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<T>(TValue value) where T: TKey
        {
            var type = typeof(T);
            if (container.TryGetValue(type, out var result))
            {
                $"重复订阅 {nameof(type)}".FrameError();
                return result;
            }
            result = new ReactiveProperty<TValue>(value);
            container.Add(type, result);
            return result;
        }
        
        public TValue Get<T>() where T: TKey 
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {nameof(type)}".FrameError();
                return default!;
            }
            return result.Value;
        }

        public void Set<T>(TValue value) where T : TKey
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {nameof(type)}".FrameError();
                return;
            }
            result.Value = value;
        }
        
        public Dictionary<Type, ReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, ReactiveProperty<TValue>>.ValueCollection Values() => container.Values;

        public bool ContainsKey<T>()  where T: class, TKey
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    public class EnumTypeSubscribe<TValue>
    {
        private readonly Dictionary<Type, ReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<T>(TValue value) where T: Enum
        {
            var type = typeof(T);
            if (container.TryGetValue(type, out var result))
            {
                $"重复订阅 {type}".FrameError();
                return result;
            }
            result = new ReactiveProperty<TValue>(value);
            container.Add(type, result);
            return result;
        }
        
        public TValue Get<T>() where T: Enum
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default!;
            }
            return result.Value;
        }
        
        public void Set<T>(TValue value) where T: Enum
        {
            var type = typeof(T);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return;
            }
            result.Value = value;
        }
        
        public Dictionary<Type, ReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, ReactiveProperty<TValue>>.ValueCollection Values() => container.Values; 

        public bool ContainsKey<T>() where T: Enum
        {
            var type = typeof(T);
            return container.ContainsKey(type);
        }
    }
    
    public class CommonSubscribe<TKey, TValue>
    {
        private readonly Dictionary<TKey, ReactiveProperty<TValue>> container = new();

        public ReactiveProperty<TValue> Subscribe(TKey key, TValue value)
        {
            if (container.TryGetValue(key, out var result))
            {
                $"重复订阅 {nameof(key)}".FrameError();
                return result;
            }
            result = new ReactiveProperty<TValue>(value);
            container.Add(key, result);
            return result;
        }

        public TValue Get(TKey key)
        {
            if (!container.TryGetValue(key, out var result))
            {
                $"未订阅 {nameof(key)}".FrameError();
                return default!;
            }
            return result.Value;
        }

        public void Set(TKey key, TValue value)
        {
            if (!container.TryGetValue(key, out var result))
            {
                $"未订阅 {nameof(key)}".FrameError();
                return;
            }
            result.Value = value;
        }

        public Dictionary<TKey, ReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<TKey, ReactiveProperty<TValue>>.ValueCollection Values() => container.Values;

        public bool ContainsKey(TKey key)
        {
            return container.ContainsKey(key);
        }
    }
}