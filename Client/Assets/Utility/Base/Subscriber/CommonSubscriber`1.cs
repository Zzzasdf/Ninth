using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.Utility
{
    public class CommonSubscriber<TKey, TValue>
    {
        public readonly CommonSubscriberContainer<TKey, TValue> Container = new();
        public readonly CommonSubscriberFuncContainer<TKey, TValue> FuncContainer = new();
    }

    public class CommonSubscriberContainer<TKey, TValue>: BaseCommonSubscriberContainer<TKey, ReactiveProperty<TValue>>
    {
        public ReactiveProperty<TValue> Subscribe(TKey key, int markBit, TValue value)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactiveProperty<TValue>(value);
            if (!Container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }

        public void Set(TKey key, int markBit, TValue value)
        {
            if (!TryGetValue(key, markBit, out var reactiveProperty))
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
                return;
            }
            reactiveProperty.Value = value;
        }
    }

    public class CommonSubscriberFuncContainer<TKey, TValue>: BaseCommonSubscriberContainer<TKey, ReactivePropertyFunc<TValue>>
    {
        public ReactivePropertyFunc<TValue> Subscribe(TKey key, int markBit, Func<TValue> valueFunc)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!Container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }
    }
    
    public abstract class BaseCommonSubscriberContainer<TKey, TReactiveProperty>
    {
        private static (TKey key, int markBit) searchCommonKey;
        protected readonly Dictionary<(TKey, int), TReactiveProperty> Container = new();

        public TReactiveProperty Get(TKey key, int markBit)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!Container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
                return default!;
            }
            return reactiveProperty;
        }
        
        public bool TryGetValue(TKey key, int markBit, out TReactiveProperty value)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return Container.TryGetValue(searchCommonKey, out value);
        }
        
        public bool ContainsKey(TKey key, int markBit)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return Container.ContainsKey(searchCommonKey);
        }

        public Dictionary<(TKey key, int markBit), TReactiveProperty>.KeyCollection Keys() => Container.Keys;
        public Dictionary<(TKey key, int markBit), TReactiveProperty>.ValueCollection Values() => Container.Values;
    }
}