

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private GameDriver() { }

        public static ProxyCtrl ProxyCtrl { get; private set; }

        private async void Awake()
        {
            DontDestroyOnLoad(this);
            "热更部分启动成功！！".Log();

           await  ProxyCtrl.JsonProxy.Set<LocalLoadConfig>();

            //// 资源加载
            //AssetsMgr = await AssetsMgr.Instance.Register();

            //AssetsMgr.TestRequest();
            //Utility.ToObjectWithLock<GameDriver>("AA").Forget();
            // Utility.ToJsonWithLock(new BundleRef(), Application.streamingAssetsPath + "/aaa/" + "jjj.json");
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    AssetsMgr.TestLoadSceneAsync();
            //}
            ////AssetsMgr.UnLoadAllAsync();
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    if(AssetsMgr != null)
            //    {
            //        AssetsMgr.CloneAsync("Assets/GAssets/LocalGroup/Cube.prefab").Forget();
            //    }
            //}
            //AssetsMgr.UnLoadAllAsync();
            //await Load();
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    DestroyImmediate(obj1);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    obj2 = await AssetsMgr.CloneAsync("Assets/GAssets/RemoteGroup/Sphere.prefab");
            //}
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    DestroyImmediate(obj2);
            //}
            //objList = null;
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    objList = new List<GameObject>();
            //    ResourceRequest req = Resources.LoadAsync("Cube");
            //    await req;
            //    GameObject m_Obj = req.asset as GameObject;
            //    objList.Add(Instantiate(m_Obj));
            //}
        }
    }
}
