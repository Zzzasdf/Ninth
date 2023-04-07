using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ninth
{
    [CreateAssetMenu(fileName = "GlobalConfigSO", menuName = "Config/GlobalConfigSO")]
    public sealed partial class GlobalConfig : ScriptableObject
    {
        // 构建模式
        public BuildMode BuildMode;

        /// <summary>
        /// 资源加载模式
        /// </summary>
        public AssetMode AssetMode;

        /// <summary>
        /// 模块列表Url
        /// </summary>
        public string Url;

        /// <summary>
        /// 本地AB包，随版本包打进StreamingAsset
        /// </summary>
        public List<string> LocalGroup;

        /// <summary>
        /// 热更AB包，随游戏启动下载
        /// </summary>
        public List<string> RemoteGroup;

        /// <summary>
        /// 运行CS脚本的模式，优先检测
        /// </summary>
        public AssetMode ScriptMode;

        // 运行Dll脚本的模式
        public AssetMode DllMode;

        private GlobalConfig()
        {
            AssetMode = AssetMode.LocalAB;
            Url = "http://192.168.8.197:80";
            LocalGroup = new List<string>()
            {
                "LocalGroup"
            };
            RemoteGroup = new List<string>()
            {
                "RemoteGroup"
            };
            ScriptMode = AssetMode.NonAB;
            DllMode = AssetMode.LocalAB | AssetMode.RemoteAB;
        }
    }

    public enum BuildMode
    {
        Debug,
        Release
    }

    public enum AssetMode
    {
        NonAB = 1 << 0,

        LocalAB = 1 << 1,

        RemoteAB = 1 << 2,
    }
}