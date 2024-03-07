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
        private readonly IReactiveProperty<TKey>? current;
        
        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (!keyCollect.IsSafetyRange(value)) return;
                currentIndex = value;
                if (current != null)
                {
                    current.Value = keyCollect[value];
                }
            }
        }

        public IEnumerable<TKey> Keys => keyCollect;
        public IEnumerable<TValue> Values => valueCollect;
        public TKey Current => keyCollect.IsSafetyRange(currentIndex) ? keyCollect[currentIndex] : default;
        public TValue? CurrentValue => valueCollect.IsSafetyRange(currentIndex) ? valueCollect[currentIndex] : default;

        public MappingSelector() : this(null)
        {
        }

        public MappingSelector(TKey current): this(new ReactiveProperty<TKey>(current))
        {
        }
        
        public MappingSelector(IReactiveProperty<TKey>? current)
        {
            this.keyCollect = new List<TKey>();
            this.valueCollect = new List<TValue>();
            this.current = current;
        }

        public TValue this[TKey key]
        {
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
            if (current != null)
            {
                if (!keyCollect.Contains(current.Value))
                {
                    $"{GetType()} 不存在当前值 {current.Value}, 已重置当前值".Warning();
                    current.Value = keyCollect[0];
                    currentIndex = 0;
                }
                else
                {
                    currentIndex = keyCollect.IndexOf(keyCollect.First(x => x.Equals(current.Value)));
                }
            }
            else
            {
                currentIndex = 0;
            }
            return this;
        }
    }
}
