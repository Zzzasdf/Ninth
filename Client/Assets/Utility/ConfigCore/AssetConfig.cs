using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    [CreateAssetMenu(fileName = "AssetConfigSO", menuName = "Config/AssetConfigSO")]
    [Serializable]
    public sealed class AssetConfig: ScriptableObject, IAssetConfig
    {
        [SerializeField] private Environment runtimeEnv;
        [SerializeField] private string url;
        
        [Inject]
        public AssetConfig()
        {
            runtimeEnv = Environment.LocalAb;
            url = "http://192.168.1.105:80";
        }
        
        Environment IAssetConfig.RuntimeEnv() => runtimeEnv;
        string IAssetConfig.Url() => url;
    }

    public enum Environment
    {
        LocalAb,
        RemoteAb,
    }
}