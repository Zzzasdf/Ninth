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

        GameObject obj1;
        GameObject obj2;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            "热更部分启动成功！！".Log();

            // 资源加载
            AssetsMgr = AssetsMgr.Instance;
        }

        private async void Update()
        {
            await AssetsMgr.UnLoadAllAsync();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                obj1 = await AssetsMgr.CloneAsync("Assets/GAssets/LocalGroup/Cube.prefab");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DestroyImmediate(obj1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                obj2 = await AssetsMgr.CloneAsync("Assets/GAssets/RemoteGroup/Sphere.prefab");
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                DestroyImmediate(obj2);
            }
        }
    }
}
