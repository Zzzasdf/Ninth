using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        private readonly PlatformConfig platformConfig;
        private readonly NameConfig nameConfig;
        private readonly PathConfig pathConfig;

        public PackConfig(PlatformConfig platformConfig, NameConfig nameConfig, PathConfig pathConfig)
        {
            this.platformConfig = platformConfig;
            this.nameConfig = nameConfig;
            this.pathConfig = pathConfig;
        }

        private string m_BuildPlatform;
        public string BuildPlatform
        {
            private get
            {
                return m_BuildPlatform;
            }
            set
            {
                m_BuildPlatform = value;
                BundleSourceDataVersionRoot =
                    string.Format("{0}/{1}/{2}", WindowSOCore.Get<WindowBuildConfig>().BuildBundlesDirectoryRoot, platformConfig.ProduceName, BuildPlatform);
                PlayerSourceDataVersionRoot =
                    string.Format("{0}/{1}/{2}", WindowSOCore.Get<WindowBuildConfig>().BuildPlayersDirectoryRoot, platformConfig.ProduceName, BuildPlatform);
            }
        }

        // 打包资源根节点
        public string GAssets { get; }
            = string.Format("{0}/GAssets", Application.dataPath);

        // 源数据版本根节点
        private string BundleSourceDataVersionRoot { get; set; }

        // 拷贝的数据根节点
        public string CopyDataRoot { get; }
            = Application.streamingAssetsPath;

        // 客户端版本根节点
        private string PlayerSourceDataVersionRoot { get; set; }
    }
}