using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public interface IAssetProxyLoad
    {
        UniTask<(AssetRef?, T?)> Get<T>(string? assetPath) where T : UnityEngine.Object;

        UniTask UnLoadAllAsync();
    }
}

