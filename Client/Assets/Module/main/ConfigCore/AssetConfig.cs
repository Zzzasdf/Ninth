using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine.Serialization;
using VContainer;

namespace Ninth
{
    [CreateAssetMenu(fileName = "AssetConfigSO", menuName = "Config/AssetConfigSO")]
    [Serializable]
    public sealed class AssetConfig: ScriptableObject, IAssetConfig
    {
        [SerializeField] private Environment buildEnv;
        [SerializeField] private Environment runtimeEnv;
        [SerializeField] private string url;
        [SerializeField] private List<string> remoteAbGroup;
        [SerializeField] private List<Environment> dllRuntimeEnv;
        
        [Inject]
        public AssetConfig()
        {
            runtimeEnv = Environment.LocalAb;
            url = "http://192.168.8.197:80";
            remoteAbGroup = new List<string> { "RemoteGroup" };
            dllRuntimeEnv = new List<Environment> { Environment.LocalAb, Environment.RemoteAb };
        }
        
        Environment IAssetConfig.BuildEnv() => buildEnv;
        Environment IAssetConfig.RuntimeEnv() => runtimeEnv;
        string IAssetConfig.Url() => url;
        ReadOnlyCollection<string> IAssetConfig.RemoteGroup() => new(remoteAbGroup);
        ReadOnlyCollection<Environment> IAssetConfig.DllRuntimeEnv() => new(dllRuntimeEnv);
    }

    public enum Environment
    {
        NonAb,
        LocalAb,
        RemoteAb,
    }
}