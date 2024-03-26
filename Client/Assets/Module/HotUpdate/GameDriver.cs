using System;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Environment = Ninth.Utility.Environment;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public class GameDriver
    {
        public static PlayerVersionConfig PlayerVersionConfig { get; set; }
        public static async void Init()
        {
#if UNITY_EDITOR
            "加载 CS 代码".Log();
            await SceneManager.LoadSceneAsync("HotUpdateScene");
#else
            "加载 Dll 代码".Log();
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            var request = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{(nameConfig as INameConfig).FileNameByVersionConfig()}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                request.error.Error();
                return;
            }
            PlayerVersionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(request.downloadHandler.text);
            var assetBundle = PlayerVersionConfig.Env switch 
            { 
                Environment.Local => AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/Remote/gassets_remotegroup"), 
                Environment.Remote => AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup"), 
                _ => throw new ArgumentOutOfRangeException()
            }; 
            await SceneManager.LoadSceneAsync("HotUpdateScene");
            assetBundle.Unload(false);
#endif
        } 
    }
}
