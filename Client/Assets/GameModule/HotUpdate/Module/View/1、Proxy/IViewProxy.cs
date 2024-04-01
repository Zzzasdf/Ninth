using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public interface IViewProxy
    {
        T Controller<T>(CancellationToken cancellationToken = default) where T : class, IViewCtrl;
        UniTask<T> ViewAsync<T>(CancellationToken cancellationToken = default) where T : BaseView;
        UniTask<TChild> ChildViewAsync<TParent, TChild>(RectTransform parentNode, CancellationToken cancellationToken) where TParent : BaseView where TChild : BaseChildView;
        UniTaskVoid RecycleAsync(VIEW_HIERARCHY hierarchy, int uniqueId, CancellationToken cancellationToken);
    }
}

