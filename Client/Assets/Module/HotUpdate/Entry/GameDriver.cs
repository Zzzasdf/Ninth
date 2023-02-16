using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    [DisallowMultipleComponent]
    public sealed class GameDriver : MonoBehaviour
    {
        public static void Init()
        {
            new GameObject("GameDriver").AddComponent<GameDriver>();
        }

        public static AssetsMgr AssetsMgr {  get ; private set; }

        private GameDriver() { }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            "热更部分启动成功8！！".Log();
            // 资源加载

            AssetsMgr = AssetsMgr.Instance;
        }
    }
}
