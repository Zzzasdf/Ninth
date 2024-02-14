using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IProcedure
    {
        UniTask<PROCEDURE> StartAsync(CancellationToken cancellationToken = default);
    }
}
