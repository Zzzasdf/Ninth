using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public interface IAssetProxy
    {
        UniTask<GameObject?> CloneAsync(string? assetPath, CancellationToken cancellationToken = default);
        UniTask<GameObject?> CloneAsync(string? assetPath, Transform? parent, CancellationToken cancellationToken = default);
        UniTask<T?> LoadAssetAsync<T>(string pathConfig, GameObject mountObj) where T : UnityEngine.Object;
        UniTask UnLoadAllAsync();
    }
}