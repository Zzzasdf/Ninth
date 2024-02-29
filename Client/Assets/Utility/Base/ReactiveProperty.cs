using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

namespace Ninth.Utility
{
    public interface IReactiveProperty<T>
    {
        public T Value { get; set; }
    }
    
    public class ReactiveProperty<T>: IReactiveProperty<T>
    {
        private T latestValue;
        private Action<T>? triggerEvents;

        public ReactiveProperty(T value)
        {
            latestValue = value;
        }
        
        T IReactiveProperty<T>.Value
        {
            get => latestValue;
            set
            {
                latestValue = value;
                triggerEvents?.Invoke(value);
            } 
        }

        public ReactiveProperty<T> AsSetEvent(Action<T> triggerEvent)
        {
            triggerEvents += triggerEvent;
            return this;
        }
    }
    
    public class ReactivePropertyFunc<T>: IReactiveProperty<T>
    {
        private readonly Func<T> valueFunc;
        public ReactivePropertyFunc(Func<T> valueFuncFunc)
        {
            valueFunc = valueFuncFunc;
        }
        T IReactiveProperty<T>.Value
        {
            get => valueFunc.Invoke();
            set { }
        }
    }

    public class PackMarkBit<TKey>
    {
        public TKey Key { get; }
        public int MarkBit { get; }
        public bool IsModify { get; }
        
        public PackMarkBit(TKey key, int markBit = 0, bool isModify = true)
        {
            this.Key = key;
            this.MarkBit = markBit;
            this.IsModify = isModify;
        }
    }
}