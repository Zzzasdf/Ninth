using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine.Serialization;
using VContainer;

namespace Ninth.Utility
{
    [CreateAssetMenu(fileName = "AssetConfigSO", menuName = "Config/AssetConfigSO")]
    [Serializable]
    public sealed class AssetConfig: ScriptableObject, IAssetConfig
    {
        [Header("Unity编辑器环境下的 LocalAb 模式\n原先挂在预制体上的 Mono 脚本只能加载 cs 代码")]
        [SerializeField] private Environment runtimeEnv;
        [SerializeField] private string url;
        [SerializeField] private List<Environment> dllRuntimeEnv;
        
        [Inject]
        public AssetConfig()
        {
            runtimeEnv = Environment.LocalAb;
            url = "http://192.168.1.105:80";
            dllRuntimeEnv = new List<Environment> { Environment.LocalAb, Environment.RemoteAb };
        }
        
        Environment IAssetConfig.RuntimeEnv() => runtimeEnv;
        string IAssetConfig.Url() => url;
        ReadOnlyCollection<Environment> IAssetConfig.DllRuntimeEnv() => new(dllRuntimeEnv);
    }

    public enum Environment
    {
        NonAb,
        LocalAb,
        RemoteAb,
    }
}