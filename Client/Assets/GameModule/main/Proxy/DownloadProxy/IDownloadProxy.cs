using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IDownloadProxy
    {
        UniTask<bool> DownloadAsync(string serverPath, string cachePath, CancellationToken cancellationToken = default);
    }
}
