using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ninth.HotUpdate
{
    public class GameDriver
    {
        public static void Init()
        {
            "UnityEditor下加载成功666!!".Log();
            AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/Remote/gassets_remotegroup");
            SceneManager.LoadScene("HotUpdateScene");
            var obj = new GameObject("GameLifetimeScope");
            GameObject.DontDestroyOnLoad(obj);
            obj.AddComponent<GameLifetimeScope>();
        } 
    }
}
