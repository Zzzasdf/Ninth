using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public class AssetProxyLoadWithNonAB: IAssetProxyLoad
    {
        public UniTask<(AssetRef?,T)> Get<T>(string assetPath) where T : Object
        {
#if UNITY_EDITOR
            return UniTask.FromResult((default(AssetRef), UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath)));
#else
            throw new NotImplementedException();
#endif
        }

        public UniTask UnLoadAllAsync()
        {
            return UniTask.CompletedTask;
        }
    }
} 
