// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Ninth.Editor
// {
//     public partial class PackConfig
//     {
//         // Local(StreamingAsset) Version
//         public string BaseVersion()
//         {
//             return pathConfig.BaseVersionPath();
//         }
//
//         // Remote Apply Version
//         public string ApplyVersionInSourceDataPath()
//         {
//             return string.Format("{0}/{1}", BundleSourceDataVersionRoot, nameConfig.FileNameByVersionConfig());
//         }
//
//         // Version
//         public string VersionInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.FileNameByVersionConfig());
//         }
//         public string VersionInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version, nameConfig.FileNameByVersionConfig());
//         }
//         public string VersionInCopyDataPath()
//         {
//             return string.Format("{0}/{1}", CopyDataRoot, nameConfig.FileNameByVersionConfig());
//         }
//
//
//         // 下载配置
//         public string DownloadConfigInRemoteInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.DownloadConfigNameByRemoteGroup());
//         }
//
//         public string DownloadConfigInRemoteInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByRemoteGroup(), nameConfig.DownloadConfigNameByRemoteGroup());
//         }
//
//         public string DownloadConfigInDllInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.DownloadConfigNameByDllGroup());
//         }
//
//         public string DownloadConfigInDllInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByDllGroup(), nameConfig.DownloadConfigNameByDllGroup());
//         }
//
//         // 加载配置
//         public string LoadConfigInLocalInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.LoadConfigNameByLocalGroup());
//         }
//
//         public string LoadConfigInLocalInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByLocalGroup(), nameConfig.LoadConfigNameByLocalGroup());
//         }
//
//         public string LoadConfigInRemoteInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.LoadConfigNameByRemoteGroup());
//         }
//
//         public string LoadConfigInRemoteInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByRemoteGroup(), nameConfig.LoadConfigNameByRemoteGroup());
//         }
//
//         public string LoadConfigInDllInSourceDataTempPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), nameConfig.LoadConfigNameByDllGroup());
//         }
//
//         public string LoadConfigInDllInSourceDataPath(string version)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByDllGroup(), nameConfig.LoadConfigNameByDllGroup());
//         }
//
//         // Bundle路径
//         public string BundleInTempInSourceDataPath(string version, string bundleName)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryTempNameByPack(), bundleName);
//         }
//
//         public string BundleInLocalInSourceDataPath(string version, string bundleName)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByLocalGroup(), bundleName);
//         }
//
//         public string BundleInRemoteInSourceDataPath(string version, string bundleName)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByRemoteGroup(), bundleName);
//         }
//
//         public string BundleInDllInSourceDataPath(string version, string bundleName)
//         {
//             return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version, nameConfig.DirectoryNameByDllGroup(), bundleName);
//         }
//     }
// }