using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public class AssetProxyLoadWithNonAB: IAssetProxyLoad
    {
        public UniTask<(AssetRef?,T?)> Get<T>(string assetPath) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return UniTask.FromResult<(AssetRef?, T?)>((default, asset));
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
