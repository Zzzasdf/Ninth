using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public interface IViewProxy
    {
        T Controller<T>(CancellationToken cancellationToken = default) where T : class, IViewCtrl;
        UniTask<T> ViewAsync<T>(CancellationToken cancellationToken = default) where T : BaseView;
        UniTaskVoid RecycleAsync(VIEW_HIERARCHY hierarchy, int uniqueId, CancellationToken cancellationToken);
    }
}

