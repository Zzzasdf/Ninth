using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public class AssetProxyLoadWithNonAB: IAssetProxyLoad
    {
        UniTask<(AssetRef?,T?)> IAssetProxyLoad.Get<T>(string? assetPath) where T : class
        {
#if UNITY_EDITOR
            if (assetPath == null)
            {
                $"无法加载：{assetPath}, 资源路径为空".FrameError();
                return UniTask.FromResult<(AssetRef?, T?)>((null, null));
            }
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return UniTask.FromResult<(AssetRef?, T?)>((null, asset));
#else
            throw new NotImplementedException();
#endif
        }

        UniTask IAssetProxyLoad.UnLoadAllAsync()
        {
            return UniTask.CompletedTask;
        }
    }
} 
