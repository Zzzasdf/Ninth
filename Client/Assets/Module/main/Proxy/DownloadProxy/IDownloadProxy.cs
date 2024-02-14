using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IDownloadProxy
    {
        UniTask<bool> DownloadAsync(string? srcPath, string? dstPath, CancellationToken cancellationToken = default);
    }
}
