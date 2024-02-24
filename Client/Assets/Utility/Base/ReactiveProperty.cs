using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

namespace Ninth.Utility
{
    public class LinkedListReactiveProperty<T>
    {
        private LinkedListNodeReactiveProperty<T>? linklistHeadNode;
        private int count;
        public bool TrySubscribe(T value, int markBit, out ReactiveProperty<T> reactiveProperty)
        {
            var node = new LinkedListNodeReactiveProperty<T>(value, markBit);
            reactiveProperty = node.ReactiveProperty;
            if (linklistHeadNode == null)
            {
                linklistHeadNode = node;
                count++;
                return true;
            }
            var isSuccess = linklistHeadNode.Subscribe(node);
            if (node.MarkBit < linklistHeadNode.MarkBit)
            {
                linklistHeadNode = node;
            }

            if (isSuccess)
            {
                count++;
            }
            return isSuccess;
        }

        public bool TryGet(int markBit, out ReactiveProperty<T>? reactiveProperty)
        {
            if (linklistHeadNode == null)
            {
                reactiveProperty = null;
                return false;
            }
            return linklistHeadNode.TryGet(markBit, out reactiveProperty);
        }

        public bool TrySet(T value, int markBit)
        {
            if (linklistHeadNode == null)
            {
                return false;
            }
            return linklistHeadNode.TrySet(value, markBit);
        }
    }
    
    public class LinkedListNodeReactiveProperty<T>
    {
        public LinkedListNodeReactiveProperty<T>? Prev { get; set; }
        public LinkedListNodeReactiveProperty<T>? Next { get; set; }
        public ReactiveProperty<T> ReactiveProperty { get; }
        public int MarkBit { get; }
        
        public LinkedListNodeReactiveProperty(T value, int markBit)
        {
            ReactiveProperty = new ReactiveProperty<T>(value);
            MarkBit = markBit;
        }

        public bool Subscribe(LinkedListNodeReactiveProperty<T> node)
        {
            if (node.MarkBit == MarkBit)
            {
                return false;
            }
            if (node.MarkBit < MarkBit)
            {
                if (Prev != null)
                {
                    Prev.Next = node;
                    node.Prev = Prev;
                }
                node.Next = this;
                Prev = node;
                return true;
            }
            if (Next == null)
            {
                Next = node;
                node.Prev = this;
                return true;
            }
            return Next.Subscribe(node);
        }

        public bool TryGet(int markBit, out ReactiveProperty<T>? reactiveProperty)
        {
            var current = this;
            while (current != null)
            {
                if (current.MarkBit == markBit)
                {
                    reactiveProperty = current.ReactiveProperty;
                    return true;
                }
                current = current.Next;
            }
            reactiveProperty = null;
            return false;
        }

        public bool TrySet(T value, int markBit)
        {
            var isSuccess = TryGet(markBit, out var reactiveProperty);
            if(!isSuccess || reactiveProperty == null)
            {
                return false;
            }
            reactiveProperty.Value = value;
            return true;
        }
    }
    
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
        
        public PackMarkBit(TKey key, int markBit = 0)
        {
            this.Key = key;
            this.MarkBit = markBit;
        }
    }
}