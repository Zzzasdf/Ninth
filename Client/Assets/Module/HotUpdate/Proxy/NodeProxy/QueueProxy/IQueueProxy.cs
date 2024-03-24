using System;

namespace Ninth.HotUpdate
{
    public interface IQueueProxy<in TKey, TValue>
        where TValue: class, INode
    {
        TValue? Dequeue<TValueExpand>(TKey key) where TValueExpand: TValue;
        TValue? Peek<TValueExpand>(TKey key) where TValueExpand: TValue;
        void Enqueue<TValueExpand>(TKey key, TValueExpand value, bool isEnqueue, Action<TValue>? removeFunc = null) where TValueExpand: TValue;
        void Clear<TValueExpand>(TKey key) where TValueExpand: TValue;
        void Clear();
    }
}
