using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public interface IViewProxy
    {
        UniTask<T?> Get<T>(CancellationToken cancellationToken = default) where T : class, IView;
        UniTask<T?> Get<T>(VIEW view, CancellationToken cancellationToken = default) where T : class, IView;
    }
}

