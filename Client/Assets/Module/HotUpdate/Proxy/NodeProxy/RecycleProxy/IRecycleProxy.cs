using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public interface IRecycleProxy<TValue>
        where TValue: INode
    {
        UniTask<TValue> GetOneAsync<TValueExpand>(Func<UniTask<TValueExpand>> newOneFunc) where TValueExpand: TValue;
        TValue GetOne<TValueExpand>(Func<TValueExpand> newOneFunc) where TValueExpand: TValue;
        void Recycle<TValueExpand>(TValueExpand value, bool isRecycle, Action<TValue>? removeFunc = null) where TValueExpand: TValue;
        void Recycle(Type key, TValue value, bool isRecycle, Action<TValue>? removeFunc = null);
        void Clear<TValueExpand>() where TValueExpand: TValue;
        void Clear();
    }
}
