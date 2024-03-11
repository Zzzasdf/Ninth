using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Ninth.Utility
{
    // 映射选择器
    public class MappingSelector<TKey, TValue>
        where TKey: struct
    {
        private readonly IList<TKey> keyCollect;
        private readonly IList<TValue> valueCollect;
        public IEnumerable<TKey> Keys => keyCollect;
        public IEnumerable<TValue> Values => valueCollect;

        public ReactiveProperty<TKey> Current { get; private set; }
        public ReactiveProperty<int> CurrentIndex { get; private set; }
        private bool isValueModify = false;
        public TValue CurrentValue => valueCollect.IsSafetyRange(CurrentIndex.Value) ? valueCollect[CurrentIndex.Value] : default!;

        public MappingSelector() : this(null)
        {
        }

        public MappingSelector(TKey current): this(new ReactiveProperty<TKey>(current))
        {
        }
        
        public MappingSelector(ReactiveProperty<TKey> current)
        {
            this.keyCollect = new List<TKey>();
            this.valueCollect = new List<TValue>();
            this.Current = current;
        }

        public TValue this[TKey key]
        {
            get => valueCollect[keyCollect.IndexOf(key)];
            set
            {
                if (keyCollect.Contains(key))
                {
                    $"无法添加重复的键：{key}".FrameError();
                    return;
                }
                keyCollect.Add(key);
                valueCollect.Add(value);
            }
        }

        public MappingSelector<TKey, TValue> Build()
        {
            if (keyCollect.Count == 0)
            {
                $"无法构建一个没有数据的 {GetType()}".FrameError();
                return this;
            }
            if (Current == null)
            {
                Current = new ReactiveProperty<TKey>(keyCollect[0]);
            }
            ReactiveProperty<int> reactiveProperty;
            if (!keyCollect.Contains(Current.Value))
            {
                $"{GetType()} 不存在当前值 {Current.Value}, 已重置当前值".Warning();
                Current.Value = keyCollect[0];
                reactiveProperty = new ReactiveProperty<int>(0);
            }
            else
            {
                reactiveProperty = new ReactiveProperty<int>(keyCollect.IndexOf(keyCollect.First(x => x.Equals(Current.Value))));
            }
            Current.AsSetEvent(value =>
            {
                if (!keyCollect.Contains(value))
                {
                    isValueModify = false;
                    return;
                }
                isValueModify = !isValueModify;
                if (isValueModify)
                {
                    CurrentIndex.Value = keyCollect.IndexOf(value);
                }
            });
            CurrentIndex = reactiveProperty.AsSetEvent(value =>
            {
                if (!keyCollect.IsSafetyRange(value))
                {
                    isValueModify = false;
                    return;
                }
                isValueModify = !isValueModify;
                if (isValueModify)
                {
                    Current.Value = keyCollect[value];
                }
            });
            return this;
        }
    }
}
