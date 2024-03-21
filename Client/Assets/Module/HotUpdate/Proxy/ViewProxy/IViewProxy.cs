using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public interface IViewProxy
    {
        T Controller<T>(CancellationToken cancellationToken = default) where T : class, IViewCtrl;
        UniTask<T> View<T>(CancellationToken cancellationToken = default) where T : UnityEngine.Component, IView;
        UniTask<T> View<T>(VIEW view, CancellationToken cancellationToken = default) where T : class, IView;
    }
}

