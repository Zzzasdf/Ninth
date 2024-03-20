using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ninth.HotUpdate
{
    public class GameDriver
    {
        public static void Init()
        {
            "HotUpdate DLl Load Success".Log();
            var assetBundle = AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup");
            SceneManager.LoadScene("HotUpdateScene");
            var obj = new GameObject("GameLifetimeScope");
            GameObject.DontDestroyOnLoad(obj);
            obj.AddComponent<GameLifetimeScope>();
            assetBundle.Unload(false);
            "finish".Log();
        } 
    }
}
