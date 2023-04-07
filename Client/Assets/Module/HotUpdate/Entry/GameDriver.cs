using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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

        public static AssetProxy AssetProxy { get; private set; }
        public static SceneProxy SceneProxy { get; private set; }
        public static DownloaderProxy DownloaderProxy { get; private set; }
        public static ViewCtrlProxy ViewCtrlProxy { get; private set; }

        private async void Awake()
        {
            DontDestroyOnLoad(this);
            "热更部分启动成功347!!".Log();

            AssetProxy = new AssetProxy();
            SceneProxy = new SceneProxy();
            DownloaderProxy = new DownloaderProxy();
            ViewCtrlProxy = new ViewCtrlProxy();

            await GameDriver.ViewCtrlProxy.Get<ControllerTest>().ShowViewTest();
        }

        private void Update()
        {
            
        }
    }
}
