using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Ninth.Utility
{
    // 集合选择器
    public class CollectSelector<T>: IEnumerable<T>
        where T: struct
    {
        private readonly IList<T> collect;
        private readonly IReactiveProperty<T>? current;
        
        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (!collect.IsSafetyRange(value)) return;
                currentIndex = value;
                if (current != null)
                {
                    current.Value = collect[value];
                }
            }
        }

        public IEnumerable<T> Keys => collect;
        public T Current => collect.IsSafetyRange(currentIndex) ? collect[currentIndex] : default;

        public CollectSelector() : this(null)
        {
        }

        public CollectSelector(T current): this(new ReactiveProperty<T>(current))
        {
        }
        
        public CollectSelector(IReactiveProperty<T>? current)
        {
            this.collect = new List<T>();
            this.current = current;
        }

        public void Add(T value)
        {
            if (collect.Contains(value))
            {
                $"无法添加重复的数据：{value}".FrameError();
                return;
            }
            collect.Add(value);
        }

        public CollectSelector<T> Build()
        {
            if (collect.Count == 0)
            {
                $"无法构建一个没有数据的 {GetType()}".FrameError();
                return this;
            }
            if (current != null)
            {
                if (!collect.Contains(current.Value))
                {
                    $"{GetType()} 不存在当前值 {current.Value}, 已重置当前值".Warning();
                    current.Value = collect[0];
                    currentIndex = 0;
                }
                else
                {
                    currentIndex = collect.IndexOf(collect.First(x => x.Equals(current.Value)));
                }
            }
            else
            {
                currentIndex = 0;
            }
            return this;
        }

        public IEnumerator<T> GetEnumerator() => collect.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
