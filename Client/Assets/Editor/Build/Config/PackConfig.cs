using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        private static string m_BuildPlatform;
        public static string BuildPlatform
        {
            private get
            {
                return m_BuildPlatform;
            }
            set
            {
                m_BuildPlatform = value;

                BundleSourceDataVersionRoot =
                    string.Format("{0}/{1}/{2}", EditorSOCore.GetBuildConfig().BuildBundlesDirectoryRoot, PlatformConfig.ProduceName, BuildPlatform);

                PlayerSourceDataVersionRoot =
                    string.Format("{0}/{1}/{2}", EditorSOCore.GetBuildConfig().BuildPlayersDirectoryRoot, PlatformConfig.ProduceName, BuildPlatform);
            }
        }

        // 打包资源根节点
        public static string GAssets { get; }
            = string.Format("{0}/GAssets", Application.dataPath);

        // 源数据版本根节点
        private static string BundleSourceDataVersionRoot { get; set; }

        // 拷贝的数据根节点
        public static string CopyDataRoot { get; }
            = Application.streamingAssetsPath;

        // 客户端版本根节点
        private static string PlayerSourceDataVersionRoot { get; set; }
    }
}