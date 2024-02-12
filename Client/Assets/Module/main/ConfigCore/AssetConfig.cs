using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace Ninth
{
    [CreateAssetMenu(fileName = "AssetConfigSO", menuName = "Config/AssetConfigSO")]
    [Serializable]
    public sealed partial class AssetConfig : ScriptableObject
    {
        // 构建模式
        [SerializeField] private BuildMode buildMode;
        public BuildMode BuildMode => buildMode;

        /// <summary>
        /// 资源加载模式
        /// </summary>
        [SerializeField] private AssetMode assetMode;
        public AssetMode AssetMode => assetMode;

        /// <summary>
        /// 模块列表Url
        /// </summary>
        [SerializeField] private string url;
        public string Url => url;

        /// <summary>
        /// 本地AB包，随版本包打进StreamingAsset
        /// </summary>
        [SerializeField] private List<string> localGroup;
        public ReadOnlyCollection<string> LocalGroup => new ReadOnlyCollection<string>(localGroup);

        /// <summary>
        /// 热更AB包，随游戏启动下载
        /// </summary>
        [SerializeField] private List<string> remoteGroup;
        public ReadOnlyCollection<string> RemoteGroup => new ReadOnlyCollection<string>(remoteGroup);

        /// <summary>
        /// 运行CS脚本的模式，优先检测
        /// </summary>
        [SerializeField] private AssetMode scriptMode;
        public AssetMode ScriptMode => scriptMode;

        // 运行Dll脚本的模式
        [SerializeField] private AssetMode dllMode;
        public AssetMode DllMode => dllMode;

        public AssetConfig()
        {
            assetMode = AssetMode.LocalAB;
            url = "http://192.168.8.197:80";
            localGroup = new List<string>()
            {
                "LocalGroup"
            };
            remoteGroup = new List<string>()
            {
                "RemoteGroup"
            };
            scriptMode = AssetMode.NonAB;
            dllMode = AssetMode.LocalAB | AssetMode.RemoteAB;
        }
    }

    public enum BuildMode
    {
        Debug,
        Release
    }

    public enum AssetMode
    {
        NonAB,
        LocalAB,
        RemoteAB,
    }
}