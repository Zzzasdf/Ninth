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

        private async void Awake()
        {
            DontDestroyOnLoad(this);
            "热更部分启动成功10！！".Log();

            // 资源加载
            AssetsMgr = AssetsMgr.Instance;
        }

        private async void Update()
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                await AssetsMgr.UnLoadAll();
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                await AssetsMgr.AssetsClearAndUnLoadAll();
            }
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject obj = await AssetsMgr.Load<GameObject>("Assets/GAssets/LocalGroup/Cube.prefab");
                obj1 = Instantiate(obj);
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                DestroyImmediate(obj1.Log(), true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameObject obj = await AssetsMgr.Load<GameObject>("Assets/GAssets/RemoteGroup/Sphere.prefab");
                obj2 = Instantiate(obj);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                DestroyImmediate(obj2.Log(), true);
            }
        }
    }
}
