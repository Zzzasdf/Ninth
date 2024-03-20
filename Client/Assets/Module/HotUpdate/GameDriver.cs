using Ninth.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                var assetBundle = AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup");
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
