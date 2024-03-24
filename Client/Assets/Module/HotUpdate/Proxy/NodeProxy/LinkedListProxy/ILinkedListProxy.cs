using System;

namespace Ninth.HotUpdate
{
    public interface ILinkedListProxy<in TKey, TValue>
        where TValue: class, INode
    {
        TValue? Dequeue<TValueExpand>(TKey key) where TValueExpand: TValue;
        TValue? Dequeue(TKey key, int uniqueId);
        void Enqueue<TValueExpand>(TKey key, TValueExpand value, bool isEnqueue, Action<TValue>? removeFunc = null) where TValueExpand: TValue;
        void Clear<TValueExpand>(TKey key) where TValueExpand: TValue;
        void Clear();
    }
}