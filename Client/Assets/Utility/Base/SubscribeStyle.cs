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
        
        public ReactiveProperty<TValue> Subscribe<TKeyExpand>(TValue value) where TKeyExpand: TKey
        {
            var type = typeof(TKeyExpand);
            if (container.TryGetValue(type, out var result))
            {
                $"重复订阅 {type}".FrameError();
                return result;
            }
            result = new ReactiveProperty<TValue>(value);
            container.Add(type, result);
            return result;
        }
        
        public TValue Get<TKeyExpand>() where TKeyExpand: TKey 
        {
            var type = typeof(TKeyExpand);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default!;
            }
            return result.Value;
        }

        public void Set<TKeyExpand>(TValue value) where TKeyExpand : TKey
        {
            var type = typeof(TKeyExpand);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return;
            }
            result.Value = value;
        }
        
        public Dictionary<Type, ReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, ReactiveProperty<TValue>>.ValueCollection Values() => container.Values;

        public bool ContainsKey<TKeyExpand>() where TKeyExpand: class, TKey
        {
            var type = typeof(TKeyExpand);
            return container.ContainsKey(type);
        }

        public bool TryGetValue<TKeyExpand>(out ReactiveProperty<TValue> value) where TKeyExpand: class, TKey
        {
            var type = typeof(TKeyExpand);
            return container.TryGetValue(type, out value);
        }
    }
    
    public class EnumTypeSubscribe<TValue>
    {
        private readonly Dictionary<Type, ReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<TKeyEnum>(TValue value) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (container.TryGetValue(type, out var result))
            {
                $"重复订阅 {type}".FrameError();
                return result;
            }
            result = new ReactiveProperty<TValue>(value);
            container.Add(type, result);
            return result;
        }
        
        public TValue Get<TKeyEnum>() where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return default!;
            }
            return result.Value;
        }
        
        public void Set<TKeyEnum>(TValue value) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (!container.TryGetValue(type, out var result))
            {
                $"未订阅 {type}".FrameError();
                return;
            }
            result.Value = value;
        }
        
        public Dictionary<Type, ReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, ReactiveProperty<TValue>>.ValueCollection Values() => container.Values; 

        public bool ContainsKey<TKeyEnum>() where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            return container.ContainsKey(type);
        }
        
        public bool TryGetValue<TKeyEnum>(out ReactiveProperty<TValue> value) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            return container.TryGetValue(type, out value);
        }
    }
    
    public class CommonSubscribe<TKey, TValue>
    {
        private readonly Dictionary<TKey, ReactiveProperty<TValue>> container = new();

        public ReactiveProperty<TValue> Subscribe(TKey key, TValue value)
        {
            if (container.TryGetValue(key, out var result))
            {
                $"重复订阅 {key!.GetType()}: {key}".FrameError();
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
                $"未订阅 {key!.GetType()}: {key}".FrameError();
                return default!;
            }
            return result.Value;
        }

        public void Set(TKey key, TValue value)
        {
            if (!container.TryGetValue(key, out var result))
            {
                $"未订阅 {key!.GetType()}: {key}".FrameError();
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
        
        public bool TryGetValue(TKey key, out ReactiveProperty<TValue> value)
        {
            return container.TryGetValue(key, out value);
        }
    }
}