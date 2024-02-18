using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IAssetBundleProxy
    {
        // UniTask<VersionConfig?> GetVersionConfig(VERSION_PATH versionPath, CancellationToken cancellationToken = default);
        // UniTask<VersionConfig?> GetVersionConfig(ASSET_SERVER_VERSION_PATH versionPath, CancellationToken cancellationToken = default);
        // UniTask<DownloadConfig?> GetDownloadConfig(CONFIG_PATH configPath, CancellationToken cancellationToken = default);
        // UniTask<DownloadConfig?> GetDownloadConfig(ASSET_SERVER_CONFIG_PATH configPath, string version, CancellationToken cancellationToken = default);
        // // UniTask DownloadBundle(BUNDLE_PATH bundlePath, string bundleName, CancellationToken cancellationToken = default);
        // UniTask<bool> DownloadBundle(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName, CancellationToken cancellationToken = default);
    }
}
