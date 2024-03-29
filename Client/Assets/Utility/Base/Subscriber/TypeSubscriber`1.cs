using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public class TypeSubscriber<TValue>
    {
        private (Type type, int markBit) searchCommonKey;
        private readonly Dictionary<(Type, int), ReactiveProperty<TValue>> Container = new();
        private readonly Dictionary<(Type, int), ReactivePropertyFunc<TValue>> FuncContainer = new();

        public ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit = 0)
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

        public ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit = 0)
        {
            var type = typeof(TKey);
            var keyGroup = (type, markBit);
            var reactiveProperty = new ReactivePropertyFunc<TValue>(valueFunc);
            if (!FuncContainer.TryAdd(keyGroup, reactiveProperty))
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }

            return reactiveProperty;
        }
        
        public TValue GetValue<TKey>(int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = typeof(TKey);
            searchCommonKey.markBit = markBit;
            if (Container.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                return reactiveProperty.Value;
            }

            if (FuncContainer.TryGetValue(searchCommonKey, out var reactivePropertyFunc))
            {
                return reactivePropertyFunc.Value;
            }
            $"未订阅 {type}, MarkBit: {markBit}".FrameError();
            return default!;
        }

        public ReactiveProperty<TValue> GetReactiveProperty<TKey>(int markBit = 0)
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

        public ReactivePropertyFunc<TValue> GetReactivePropertyFunc<TKey>(int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            if (!FuncContainer.TryGetValue(searchCommonKey, out var reactiveProperty))
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return default!;
            }

            return reactiveProperty;
        }

        public bool TryGetValue<TKey>(out ReactiveProperty<TValue> value, int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return Container.TryGetValue(searchCommonKey, out value);
        }

        public bool TryGetFuncValue<TKey>(out ReactivePropertyFunc<TValue> value, int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return FuncContainer.TryGetValue(searchCommonKey, out value);
        }

        public bool ContainsKey<TKey>(int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return Container.ContainsKey(searchCommonKey);
        }
        
        public bool ContainsFuncKey<TKey>(int markBit = 0)
        {
            var type = typeof(TKey);
            searchCommonKey.type = type;
            searchCommonKey.markBit = markBit;
            return FuncContainer.ContainsKey(searchCommonKey);
        }

        public Dictionary<(Type type, int markBit), ReactiveProperty<TValue>>.KeyCollection Keys() => Container.Keys;
        public Dictionary<(Type type, int markBit), ReactiveProperty<TValue>>.ValueCollection Values() => Container.Values;
        public Dictionary<(Type type, int markBit), ReactivePropertyFunc<TValue>>.KeyCollection FuncKeys() => FuncContainer.Keys;
        public Dictionary<(Type type, int markBit), ReactivePropertyFunc<TValue>>.ValueCollection FuncValues() => FuncContainer.Values;
    }
}