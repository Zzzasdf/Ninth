using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Ninth
{
    public class GameEntry : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            new GameObject("GameLifetimeScope").AddComponent<GameLifetimeScope>();
        }
#else
        public static NameConfig NameConfig { get; set; }
        public static PlayerVersionConfig PlayerVersionConfig { get; set; }
        private async void Awake()
        {
            NameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            var request = UnityWebRequest.Get($"{Application.streamingAssetsPath}/{(NameConfig as INameConfig).FileNameByVersionConfig()}");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                request.error.Error();
                return;
            }
            PlayerVersionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(request.downloadHandler.text);
            new GameObject("GameLifetimeScope").AddComponent<GameLifetimeScope>();
        }
#endif
    }
}
