using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr : Singleton<AssetsMgr>
    {
        // 资源配置
        private Dictionary<string, AssetRef> m_ConfigAssetPath2AssetRef;

        // 加载进内存的bundle, 确保只存在一个
        public Dictionary<string, BundleRef> m_BundlePath2BundleRef;

        private Func<string, string> m_LocalPathFunc;
        private Func<string, string> m_RemotePathFunc;

        public async UniTask<AssetsMgr> Init()
        {
            m_ConfigAssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundlePath2BundleRef = new Dictionary<string, BundleRef>();
            await Register();
            return this;
        }
    }
}