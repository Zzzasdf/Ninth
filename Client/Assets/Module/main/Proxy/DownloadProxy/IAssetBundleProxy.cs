using Cysharp.Threading.Tasks;

namespace Ninth
{
    public interface IAssetBundleProxy
    {
        UniTask<VersionConfig?> GetVersionConfig(VERSION_PATH versionPath);
        UniTask<VersionConfig?> GetVersionConfig(ASSET_SERVER_VERSION_PATH versionPath);
    }
}
