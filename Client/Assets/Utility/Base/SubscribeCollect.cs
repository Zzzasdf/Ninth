using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace Ninth.Utility
{
    public class SubscribeCollect<TValue> : SubscribeCollect<TValue, Enum>
    {
    }

    public class SubscribeCollect<TValue, TEnum> : BaseSubscribeCollect<TValue, TEnum>
        where TEnum : Enum
    {
        public new ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit = 0) => base.Subscribe<TKey>(value, markBit);
        public new ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit = 0) => base.Subscribe<TKey>(valueFunc, markBit);
        public new ReactiveProperty<TValue> Subscribe(TEnum key, TValue value, int markBit = 0) => base.Subscribe(key, value, markBit);
        public new ReactivePropertyFunc<TValue> Subscribe(TEnum key, Func<TValue> valueFunc, int markBit = 0) => base.Subscribe(key, valueFunc, markBit);
        public new TValue Get<TKey>(int markBit = 0) => base.Get<TKey>(markBit);
        public new TValue Get(TEnum key, int markBit = 0) => base.Get(key, markBit);
        public new void Set<TKey>(TValue value, int markBit = 0) => base.Set<TKey>(value, markBit);
        public new void Set(TEnum key, TValue value, int markBit = 0) => base.Set(key, value, markBit);
        public new bool TryGetValue<TKey>(out TValue value, int markBit = 0) => base.TryGetValue<TKey>(out value, markBit);
        public new bool TryGetValue(TEnum key, out TValue value, int markBit = 0) => base.TryGetValue(key, out value, markBit);
        public new bool ContainsKey<TKey>(int markBit = 0) => base.ContainsKey<TKey>(markBit);
        public new bool ContainsKey(TEnum key, int markBit = 0) => base.ContainsKey(key, markBit);
        public new Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.KeyCollection Keys() => base.Keys();
        public new Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.ValueCollection Values() => base.Values();
        public new Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.KeyCollection KeysByCommon() => base.KeysByCommon();
        public new Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.ValueCollection ValuesByCommon() => base.ValuesByCommon();
    }

    public class SubscribeCollect<TValue, TKey, TEnumType>: SubscribeCollect<TValue, TKey, TEnumType, Enum>
        where TEnumType : Enum
    {
    }

    public class SubscribeCollect<TValue, TKey, TEnumType, TEnum> : BaseSubscribeCollect<TValue, TEnum>
        where TEnumType : Enum
        where TEnum : Enum
    {
        public new ReactiveProperty<TValue> Subscribe<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand : TKey => base.Subscribe<TKeyExpand>(value, markBit);
        public new ReactivePropertyFunc<TValue> Subscribe<TKeyExpand>(Func<TValue> valueFunc, int markBit = 0) where TKeyExpand : TKey => base.Subscribe<TKeyExpand>(valueFunc, markBit);
        public new ReactivePropertyFunc<TValue> SubscribeByEnumType<TEnumTypeExpand>(Func<TValue> valueFunc, int markBit = 0) where TEnumTypeExpand : TEnumType => base.Subscribe<TEnumTypeExpand>(valueFunc, markBit);
        public new ReactiveProperty<TValue> SubscribeByEnumType<TEnumTypeExpand>(TValue value, int markBit = 0) where TEnumTypeExpand : TEnumType => base.Subscribe<TEnumTypeExpand>(value, markBit);
        public new ReactiveProperty<TValue> Subscribe(TEnum key, TValue value, int markBit = 0) => base.Subscribe(key, value, markBit);
        public new ReactivePropertyFunc<TValue> Subscribe(TEnum key, Func<TValue> valueFunc, int markBit = 0) => base.Subscribe(key, valueFunc, markBit);
        public new TValue Get<TKeyExpand>(int markBit = 0) where TKeyExpand: TKey => base.Get<TKeyExpand>(markBit);
        public new TValue GetByEnumType<TEnumTypeExpand>(int markBit = 0) where TEnumTypeExpand: TEnumType => base.Get<TEnumTypeExpand>(markBit);
        public new TValue Get(TEnum key, int markBit = 0) => base.Get(key, markBit);
        public new void Set<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand: TKey => base.Set<TKeyExpand>(value, markBit);
        public new void SetByEnumType<TEnumTypeExpand>(TValue value, int markBit = 0) where TEnumTypeExpand: TEnumType => base.Set<TEnumTypeExpand>(value, markBit);
        public new void Set(TEnum key, TValue value, int markBit = 0) => base.Set(key, value, markBit);
        public new bool TryGetValue<TKeyExpand>(out TValue value, int markBit = 0) where TKeyExpand: TKey => base.TryGetValue<TKeyExpand>(out value, markBit);
        public new bool TryGetValueByEnumType<TEnumTypeExpand>(out TValue value, int markBit = 0) where TEnumTypeExpand: TEnumType => base.TryGetValue<TEnumTypeExpand>(out value, markBit);
        public new bool TryGetValue(TEnum key, out TValue value, int markBit = 0) => base.TryGetValue(key, out value, markBit);
        public new bool ContainsKey<TKeyExpand>(int markBit = 0) where TKeyExpand: TKey => base.ContainsKey<TKeyExpand>(markBit);
        public new bool ContainsKeyByEnumType<TEnumTypeExpand>(int markBit = 0) where TEnumTypeExpand: TEnumType => base.ContainsKey<TEnumTypeExpand>(markBit);
        public new bool ContainsKey(TEnum key, int markBit = 0) => base.ContainsKey(key, markBit);
        public new Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.KeyCollection Keys() => base.Keys();
        public new Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.ValueCollection Values() => base.Values();
        public new Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.KeyCollection KeysByCommon() => base.KeysByCommon();
        public new Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.ValueCollection ValuesByCommon() => base.ValuesByCommon();
    }
}