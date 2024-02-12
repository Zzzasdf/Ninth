using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IProcedure
    {
        UniTask<ProcedureInfo> StartAsync(CancellationToken cancellationToken = default);
    }
}
