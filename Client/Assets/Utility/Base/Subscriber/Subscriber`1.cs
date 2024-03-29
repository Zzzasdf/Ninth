using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.Utility
{
    public class Subscriber<TKey, TValue>
    {
        private static (TKey key, int markBit) searchCommonKey;
        private readonly Dictionary<(TKey, int), ReactiveProperty<TValue>> Container = new();
        private readonly Dictionary<(TKey, int), ReactivePropertyFunc<TValue>> FuncContainer = new();

        public ReactiveProperty<TValue> Subscribe(TKey key, TValue value, int markBit = 0)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactiveProperty<TValue>(value);
            if (!Container.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }

        public ReactivePropertyFunc<TValue> Subscribe(TKey key, Func<TValue> valueFunc, int markBit = 0)
        {
            var keyGroup = (key, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!FuncContainer.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {key}, MarkBit: {markBit}".FrameError();
            }
            return reactiveProperty;
        }

        public TValue GetValue(TKey key, int markBit = 0)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (Container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                return reactiveProperty.Value;
            }
            if (FuncContainer.TryGetValue(searchCommonKey, out var reactivePropertyFunc))
            {
                return reactivePropertyFunc.Value;
            }
            $"未订阅 {key}, MarkBit: {markBit}".FrameError();
            return default!;
        }

        public ReactiveProperty<TValue> GetReactiveProperty(TKey key, int markBit = 0)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!Container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {key}, MarkBit: {markBit}".FrameError();
                return default!;
            }

            return reactiveProperty;
        }

        public ReactivePropertyFunc<TValue> GetReactivePropertyFunc(TKey key, int markBit = 0)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            if (!FuncContainer.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {key}, MarkBit: {markBit}".FrameError();
                return default!;
            }
            return reactiveProperty;
        }

        public bool TryGetValue(TKey key, int markBit, out ReactiveProperty<TValue> value)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return Container.TryGetValue(searchCommonKey, out value);
        }

        public bool TryGetValue(TKey key, int markBit, out ReactivePropertyFunc<TValue> value)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return FuncContainer.TryGetValue(searchCommonKey, out value);
        }

        public bool ContainsKey(TKey key, int markBit = 0)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return Container.ContainsKey(searchCommonKey);
        }

        public bool ContainsFuncKey(TKey key, int markBit = 0)
        {
            searchCommonKey.key = key;
            searchCommonKey.markBit = markBit;
            return FuncContainer.ContainsKey(searchCommonKey);
        }

        public Dictionary<(TKey key, int markBit), ReactiveProperty<TValue>>.KeyCollection Keys() => Container.Keys;
        public Dictionary<(TKey key, int markBit), ReactiveProperty<TValue>>.ValueCollection Values() => Container.Values;
        public Dictionary<(TKey key, int markBit), ReactiveProperty<TValue>>.KeyCollection FuncKeys() => Container.Keys;
        public Dictionary<(TKey key, int markBit), ReactiveProperty<TValue>>.ValueCollection FuncValues() => Container.Values;
    }
}