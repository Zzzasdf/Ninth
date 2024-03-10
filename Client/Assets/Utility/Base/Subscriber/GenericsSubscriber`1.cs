using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public class GenericsSubscriber<TValue>
    {
        public readonly GenericsSubscriberContainer<TValue> Container = new();
        public readonly GenericsSubscriberFuncContainer<TValue> FuncContainer = new();
    }
    
    public class GenericsSubscriberContainer<TValue> : BaseGenericsSubscriberContainer<ReactiveProperty<TValue>>
    {
        public ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit)
        {
            var type = typeof(TKey);
            var keyGroup = (type, markBit);
            var reactiveProperty = new ReactiveProperty<TValue>(value);
            if (!Container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }

            return reactiveProperty;
        }

        public void Set<TKey>(int markBit, TValue value)
        {
            if (!TryGetValue<TKey>(markBit, out var reactiveProperty))
            {
                $"未订阅 {typeof(TKey)}, MarkBit: {markBit}".FrameError();
                return;
            }
            reactiveProperty.Value = value;
        }
    }
    
    public class GenericsSubscriberFuncContainer<TValue>: BaseGenericsSubscriberContainer<ReactivePropertyFunc<TValue>>
    {
        public ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit)
        {
            var type = typeof(TKey);
            var keyGroup = (type, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!Container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }
    }

    public abstract class BaseGenericsSubscriberContainer<TReactiveProperty>
    {
        private (Type type, int markBit) searchCommonKey;
        protected readonly Dictionary<(Type, int), TReactiveProperty> Container = new();
        
        public TReactiveProperty Get<TKey>(int markBit)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            if (!Container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return default!;
            }
            return reactiveProperty;
        }
        
        public bool TryGetValue<TKey>(int markBit, out TReactiveProperty value)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return Container.TryGetValue(searchCommonKey, out value);
        }

        public bool ContainsKey<TKey>(int markBit)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return Container.ContainsKey(searchCommonKey);
        }
        
        public Dictionary<(Type type, int markBit), TReactiveProperty>.KeyCollection Keys() => Container.Keys;
        public Dictionary<(Type type, int markBit), TReactiveProperty>.ValueCollection Values() => Container.Values;
    }
}