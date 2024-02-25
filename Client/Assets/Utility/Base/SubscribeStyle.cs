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
        private readonly Dictionary<Type, LinkedListReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand: TKey
        {
            var type = typeof(TKeyExpand);
            if (!container.TryGetValue(type, out var linkedList))
            {
                linkedList = new LinkedListReactiveProperty<TValue>();
                container.Add(type, linkedList);
            }
            var isSuccess = linkedList.TrySubscribe(value, markBit, out var result);
            if (!isSuccess)
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }
            return result;
        }
        
        public TValue? Get<TKeyExpand>(int markBit = 0) where TKeyExpand: TKey 
        {
            var type = typeof(TKeyExpand);
            if (!container.TryGetValue(type, out var linkedList))
            {
                $"未订阅 {type}".FrameError();
                return default;
            }
            var isSuccess = linkedList.TryGet(markBit, out var result);
            if (!isSuccess || result == null)
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return default;
            }
            return result.Value;
        }

        public void Set<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand : TKey
        {
            var type = typeof(TKeyExpand);
            if (!container.TryGetValue(type, out var linkedList))
            {
                $"未订阅 {type}".FrameError();
                return;
            }
            var isSuccess = linkedList.TrySet(value, markBit);
            if (!isSuccess)
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
            }
        }
        
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.ValueCollection Values() => container.Values;

        public bool ContainsKey<TKeyExpand>() where TKeyExpand: class, TKey
        {
            var type = typeof(TKeyExpand);
            return container.ContainsKey(type);
        }

        public bool TryGetValue<TKeyExpand>(out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0) where TKeyExpand: class, TKey
        {
            var type = typeof(TKeyExpand);
            var result = container.TryGetValue(type, out var linkedList);
            if (!result || linkedList == null)
            {
                reactiveProperty = null;
                return result;
            }
            return linkedList.TryGet(markBit, out reactiveProperty);
        }
    }
    
    public class EnumTypeSubscribe<TValue>
    {
        private readonly Dictionary<Type, LinkedListReactiveProperty<TValue>> container = new();
        
        public ReactiveProperty<TValue> Subscribe<TKeyEnum>(TValue value, int markBit = 0) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (!container.TryGetValue(type, out var linkedList))
            {
                linkedList = new LinkedListReactiveProperty<TValue>();
                container.Add(type, linkedList);
            }
            var isSuccess = linkedList.TrySubscribe(value, markBit, out var result);
            if (!isSuccess)
            {
                $"重复订阅 {type}, MarkBit: {markBit}".FrameError();
            }
            return result;
        }
        
        public TValue? Get<TKeyEnum>(int markBit = 0) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (!container.TryGetValue(type, out var linkedList))
            {
                return default;
            }
            var isSuccess = linkedList.TryGet(markBit, out var result);
            if (!isSuccess || result == null)
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
                return default;
            }
            return result.Value;
        }
        
        public void Set<TKeyEnum>(TValue value, int markBit = 0) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            if (!container.TryGetValue(type, out var linkedList))
            {
                $"未订阅 {type}".FrameError();
                return;
            }
            var isSuccess = linkedList.TrySet(value, markBit);
            if (!isSuccess)
            {
                $"未订阅 {type}, MarkBit: {markBit}".FrameError();
            }
        }
        
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.ValueCollection Values() => container.Values; 

        public bool ContainsKey<TKeyEnum>() where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            return container.ContainsKey(type);
        }
        
        public bool TryGetValue<TKeyEnum>(out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0) where TKeyEnum: Enum
        {
            var type = typeof(TKeyEnum);
            var result = container.TryGetValue(type, out var linkedList);
            if (!result || linkedList == null)
            {
                reactiveProperty = null;
                return result;
            }
            return linkedList.TryGet(markBit, out reactiveProperty);
        }
    }
    
    public class CommonSubscribe<TKey, TValue>
    {
        private readonly Dictionary<TKey, LinkedListReactiveProperty<TValue>> container = new();

        public ReactiveProperty<TValue> Subscribe(TKey key, TValue value, int markBit = 0)
        {
            if (!container.TryGetValue(key, out var linkedList))
            {
                linkedList = new LinkedListReactiveProperty<TValue>();
                container.Add(key, linkedList);
            }
            var isSuccess = linkedList.TrySubscribe(value, markBit, out var result);
            if (!isSuccess)
            {
                $"重复订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
            return result;
        }

        public TValue? Get(TKey key, int markBit = 0)
        {
            if (!container.TryGetValue(key, out var linkedList))
            {
                $"未订阅 {key!.GetType()}: {key}".FrameError();
                return default;
            }
            var isSuccess = linkedList.TryGet(markBit, out var result);
            if (!isSuccess || result == null)
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
                return default;
            }
            return result.Value;
        }

        public void Set(TKey key, TValue value, int markBit = 0)
        {
            if (!container.TryGetValue(key, out var linkedList))
            {
                $"未订阅 {key!.GetType()}: {key}".FrameError();
                return;
            }
            var isSuccess = linkedList.TrySet(value, markBit);
            if (!isSuccess)
            {
                $"未订阅 {key!.GetType()}: {key}, MarkBit: {markBit}".FrameError();
            }
        }

        public Dictionary<TKey, LinkedListReactiveProperty<TValue>>.KeyCollection Keys() => container.Keys;
        public Dictionary<TKey, LinkedListReactiveProperty<TValue>>.ValueCollection Values() => container.Values;

        public bool ContainsKey(TKey key)
        {
            return container.ContainsKey(key);
        }
        
        public bool TryGetValue(TKey key, out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0)
        {
            var result = container.TryGetValue(key, out var linkedList);
            if (!result || linkedList == null)
            {
                reactiveProperty = null;
                return result;
            }
            return linkedList.TryGet(markBit, out reactiveProperty);
        }
    }
}