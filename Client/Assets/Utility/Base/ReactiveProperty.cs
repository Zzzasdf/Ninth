using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

namespace Ninth.Utility
{
    public class ReactiveProperty<T>
    {
        private T latestValue;
        private Action<T>? triggerEvents;

        public ReactiveProperty(T value)
        {
            latestValue = value;
        }
        
        public T Value
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