using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public class GenericsSubscribe<TValue>
    {
        private (Type type, int markBit) searchCommonKey;
        private readonly Dictionary<(Type, int), IReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit)
        {
            var type = typeof(TKey);
            var keyGroup = (type, markBit);
            var reactiveProperty = new ReactiveProperty<TValue>(value);
            if (!container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }

        public ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit)
        {
            var type = typeof(TKey);
            var keyGroup = (type, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }
        
        public TValue Get<TKey>(int markBit)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return default;
            }
            return reactiveProperty.Value;
        }

        public void Set<TKey>(int markBit, TValue value)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return;
            }
            reactiveProperty.Value = value;
        }
        
        public bool TryGetValue<TKey>(int markBit, out TValue value)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                value = default;
                return false;
            }
            value = reactiveProperty.Value;
            return true;
        }

        public bool ContainsKey<TKey>(int markBit)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return container.ContainsKey(searchCommonKey);
        }
        
        public Dictionary<(Type type, int markBit), IReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<(Type type, int markBit), IReactiveProperty<TValue>>.ValueCollection Values() => container.Values;
    }
    
    public class CommonSubscribe<TKey, TValue>
    {
        private static (TKey key, int markBit) searchCommonKey;
        private readonly Dictionary<(TKey, int), IReactiveProperty<TValue>> container = new();

        public ReactiveProperty<TValue> Subscribe(TKey key, int markBit, TValue value)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactiveProperty<TValue>(value);
            if (!container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }
        
        public ReactivePropertyFunc<TValue> Subscribe(TKey key, int markBit, Func<TValue> valueFunc)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }

        public TValue? Get(TKey key, int markBit)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
                return default;
            }
            return reactiveProperty.Value;
        }

        public void Set(TKey key, int markBit, TValue value)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
                return;
            }
            reactiveProperty.Value = value;
        }
        
        public bool TryGetValue(TKey key, int markBit, out TValue value)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                value = default;
                return false;
            }
            value = reactiveProperty.Value;
            return true;
        }
        
        public bool ContainsKey(TKey key, int markBit)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return container.ContainsKey(searchCommonKey);
        }

        public Dictionary<(TKey key, int markBit), IReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<(TKey key, int markBit), IReactiveProperty<TValue>>.ValueCollection Values() => container.Values;
    }
}