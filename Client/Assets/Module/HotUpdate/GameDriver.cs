using System;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment = Ninth.Utility.Environment;

namespace Ninth.HotUpdate
{
    public class GameDriver
    {
        public static void Init()
        {
            "HotUpdate Module Load Success".Log();
            IAssetConfig assetConfig = Resources.Load<AssetConfig>("SOData/AssetConfigSO");
            if(assetConfig.DllRuntimeEnv().Contains(assetConfig.RuntimeEnv()))
            {
                var assetBundle = assetConfig.RuntimeEnv() switch
                {
                    Environment.LocalAb => AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/Remote/gassets_remotegroup"),
                    Environment.RemoteAb => AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup"),
                    _ => throw new ArgumentOutOfRangeException()
                };
                SceneManager.LoadScene("HotUpdateScene");
                var obj = new GameObject("GameLifetimeScope");
                GameObject.DontDestroyOnLoad(obj);
                obj.AddComponent<GameLifetimeScope>();
                assetBundle.Unload(false);
            }
            else
            {
                SceneManager.LoadScene("HotUpdateScene");
                var obj = new GameObject("GameLifetimeScope");
                GameObject.DontDestroyOnLoad(obj);
                obj.AddComponent<GameLifetimeScope>();
            }
            "finish".Log();
        } 
    }
}
