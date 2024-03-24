using System;

namespace Ninth.HotUpdate
{
    public interface IStackProxy<in TKey, TValue>
        where TValue: class, INode
    {
        TValue? Peek(TKey key);
        TValue? Pop(TKey key);
        void Push<TValueExpand>(TKey key, TValueExpand value, bool isPush, Action<TValue>? removeFunc = null) where TValueExpand: TValue;
        void Clear<TValueExpand>(TKey key) where TValueExpand: TValue;
        void Clear();
    }
}
