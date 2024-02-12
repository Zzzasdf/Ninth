using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Ninth
{
    [CreateAssetMenu(fileName = "AssetConfigSO", menuName = "Config/AssetConfigSO")]
    [Serializable]
    public sealed class AssetConfig : ScriptableObject
    {
        // 构建模式
        [SerializeField] private BuildMode buildMode;
        public BuildMode BuildMode => buildMode;

        /// <summary>
        /// 运行环境
        /// </summary>
        [SerializeField] private RuntimeEnv runtimeEnv;
        public RuntimeEnv RuntimeEnv => runtimeEnv;

        /// <summary>
        /// 模块列表Url
        /// </summary>
        [SerializeField] private string url;
        public string Url => url;

        /// <summary>
        /// 热更AB包，随游戏启动下载
        /// </summary>
        [SerializeField] private List<string> remoteAbGroup;
        public ReadOnlyCollection<string> RemoteAbGroup => new(remoteAbGroup);

        // 运行Dll脚本的模式
        [SerializeField] private List<RuntimeEnv> dllRuntimeEnv;
        public List<RuntimeEnv> DllRuntimeEnv => dllRuntimeEnv;

        public AssetConfig()
        {
            runtimeEnv = RuntimeEnv.LocalAb;
            url = "http://192.168.8.197:80";
            remoteAbGroup = new List<string> { "RemoteGroup" };
            dllRuntimeEnv = new List<RuntimeEnv> { RuntimeEnv.LocalAb, RuntimeEnv.RemoteAb };
        }
    }

    public enum BuildMode
    {
        Debug,
        Release
    }
    
    public enum RuntimeEnv
    {
        NonAb,
        LocalAb,
        RemoteAb,
    }
}