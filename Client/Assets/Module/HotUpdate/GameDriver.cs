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
            "UnityEditor下加载成功 999".Log();
            AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup");
            SceneManager.LoadScene("HotUpdateScene");
            
            var obj = new GameObject("GameLifetimeScope");
            GameObject.DontDestroyOnLoad(obj);
            obj.AddComponent<GameLifetimeScope>();

            var bundle = AssetBundle.LoadFromFile($"{Application.persistentDataPath}/Remote/gassets_remotegroup_test");
            var sphere = bundle.LoadAsset("Sphere") as GameObject;
            GameObject.Instantiate(sphere);

            new GameObject("AAA").AddComponent<HotUpdateMono>();
            "finish".Log();
        } 
    }
}
