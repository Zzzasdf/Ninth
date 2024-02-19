// using UnityEngine;
//
// namespace Ninth.Editor
// {
//     public partial class PackConfig
//     {
//         private readonly IPlayerSettingsConfig playerSettingsConfig;
//         private readonly INameConfig nameConfig;
//
//         public PackConfig(IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
//         {
//             this.playerSettingsConfig = playerSettingsConfig;
//             this.nameConfig = nameConfig;
//         }
//
//         private string m_BuildPlatform;
//         public string BuildPlatform
//         {
//             private get
//             {
//                 return m_BuildPlatform;
//             }
//             set
//             {
//                 m_BuildPlatform = value;
//                 BundleSourceDataVersionRoot =
//                     string.Format("{0}/{1}/{2}", WindowSOCore.Get<WindowBuildSO>().BuildBundlesTargetFolderRoot, playerSettingsConfig.Get(PLAY_SETTINGS.ProduceName), BuildPlatform);
//                 PlayerSourceDataVersionRoot =
//                     string.Format("{0}/{1}/{2}", WindowSOCore.Get<WindowBuildSO>().BuildPlayersDirectoryRoot, playerSettingsConfig.Get(PLAY_SETTINGS.ProduceName), BuildPlatform);
//             }
//         }
//
//         // 打包资源根节点
//         public string GAssets { get; }
//             = string.Format("{0}/GAssets", Application.dataPath);
//
//         // 源数据版本根节点
//         private string BundleSourceDataVersionRoot { get; set; }
//
//         // 拷贝的数据根节点
//         public string CopyDataRoot { get; }
//             = Application.streamingAssetsPath;
//
//         // 客户端版本根节点
//         private string PlayerSourceDataVersionRoot { get; set; }
//     }
// }