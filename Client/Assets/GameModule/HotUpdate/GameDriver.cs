using System;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Environment = Ninth.Utility.Environment;

namespace Ninth.HotUpdate
{
    public class GameDriver
    {
        public static PreLoadAssets PreLoadAssets { get; set; }
#if UNITY_EDITOR
        public static async void Init()
        {
            "加载 CS 代码".Log();
            PreLoadAssets = UnityEditor.AssetDatabase.LoadAssetAtPath<PreLoadAssets>("Assets/GAssets/RemoteGroup/Preload/PreLoadAssetsSO.asset");
            await SceneManager.LoadSceneAsync("HotUpdateScene");
        }
#else
        public static NameConfig NameConfig { get; set; }
        public static PlayerVersionConfig PlayerVersionConfig { get; set; }
        public static async void Init()
        {
            "加载 Dll 代码".Log();
            NameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            var request = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{(NameConfig as INameConfig).FileNameByVersionConfig()}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                request.error.Error();
                return;
            }
            PlayerVersionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(request.downloadHandler.text);
            var prefix = PlayerVersionConfig.Env switch 
            { 
                Environment.Local => Application.streamingAssetsPath, 
                Environment.Remote => Application.persistentDataPath, 
                _ => throw new ArgumentOutOfRangeException()
            };
            var preloadBundle = AssetBundle.LoadFromFile($"{prefix}/Remote/gassets_remotegroup_preload");
            PreLoadAssets = preloadBundle.LoadAsset<PreLoadAssets>("Assets/GAssets/RemoteGroup/Preload/PreLoadAssetsSO.asset");
            await preloadBundle.UnloadAsync(false);
            
            var assetBundle = AssetBundle.LoadFromFile($"{prefix}/Remote/gassets_remotegroup");
            await SceneManager.LoadSceneAsync("HotUpdateScene");
            assetBundle.Unload(false);
        } 
#endif
    }
}
