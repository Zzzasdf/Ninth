using System.Collections;
using System.Collections.Generic;

namespace Ninth
{
    public static class AssetBundleModuleConfig
    {
        /// <summary>
        /// 本地AB包，随版本包打进StreamingAsset
        /// </summary>
        public static List<string> LocalGroup { get; }
            = new List<string> { "LocalGroup" };

        /// <summary>
        /// 热更AB包，随游戏启动下载
        /// </summary>
        public static List<string> RemoteGroup { get; }
            = new List<string> { "RemoteGroup" };
    }
}

