using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public interface IAssetProxyLoad
    {
        UniTask<(AssetRef?, T?)> Get<T>(string? assetPath) where T : UnityEngine.Object;

        UniTask UnLoadAllAsync();
    }
}

