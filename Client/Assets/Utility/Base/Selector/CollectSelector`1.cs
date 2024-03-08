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
    {
        private readonly IList<T> collect;
        public IEnumerable<T> Collect => collect;

        
        public ReactiveProperty<T> Current { get; private set; }
        public ReactiveProperty<int> CurrentIndex { get; private set; }
        private bool isValueModify = false;
        public T CurrentValue => collect.IsSafetyRange(CurrentIndex.Value) ? collect[CurrentIndex.Value] : default!;

        public CollectSelector() : this(null)
        {
        }

        public CollectSelector(T current): this(new ReactiveProperty<T>(current))
        {
        }
        
        public CollectSelector(ReactiveProperty<T> current)
        {
            this.collect = new List<T>();
            this.Current = current;
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
            if (Current == null)
            {
                Current = new ReactiveProperty<T>(collect[0]);
            }
            ReactiveProperty<int> reactiveProperty;

            if (!collect.Contains(Current.Value))
            {
                $"{GetType()} 不存在当前值 {Current.Value}, 已重置当前值".Warning();
                Current.Value = collect[0];
                reactiveProperty = new ReactiveProperty<int>(0);
            }
            else
            {
                reactiveProperty = new ReactiveProperty<int>(collect.IndexOf(collect.First(x => x.Equals(Current.Value))));
            }
            Current.AsSetEvent(value =>
            {
                if (!collect.Contains(value))
                {
                    isValueModify = false;
                    return;
                }
                isValueModify = !isValueModify;
                if (isValueModify)
                {
                    CurrentIndex.Value = collect.IndexOf(value);
                }
            });
            CurrentIndex = reactiveProperty.AsSetEvent(value =>
            {
                if (!collect.IsSafetyRange(value))
                {
                    isValueModify = false;
                    return;
                }
                isValueModify = !isValueModify;
                if (isValueModify)
                {
                    Current.Value = collect[value];
                }
            });
            return this;
        }

        public IEnumerator<T> GetEnumerator() => collect.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
