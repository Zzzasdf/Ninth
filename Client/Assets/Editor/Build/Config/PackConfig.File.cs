using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        // Local(StreamingAsset) Version
        public static string BaseVersion()
        {
            return PathConfig.BaseVersionPath();
        }

        // Remote Apply Version
        public static string ApplyVersionInSourceDataPath()
        {
            return string.Format("{0}/{1}", BundleSourceDataVersionRoot, NameConfig.VersionConfigName);
        }

        // Version
        public static string VersionInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.VersionConfigName);
        }
        public static string VersionInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.VersionConfigName);
        }
        public static string VersionInCopyDataPath()
        {
            return string.Format("{0}/{1}", CopyDataRoot, NameConfig.VersionConfigName);
        }


        // 下载配置
        public static string DownloadConfigInRemoteInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.DownloadConfigNameInRemote);
        }

        public static string DownloadConfigInRemoteInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.DownloadConfigNameInRemote);
        }

        public static string DownloadConfigInDllInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.DownloadConfigNameInDll);
        }

        public static string DownloadConfigInDllInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.DownloadConfigNameInDll);
        }

        // 加载配置
        public static string LoadConfigInLocalInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.LoadConfigNameInLocal);
        }

        public static string LoadConfigInLocalInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.LocalDirectory, NameConfig.LoadConfigNameInLocal);
        }

        public static string LoadConfigInRemoteInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.LoadConfigNameInRemote);
        }

        public static string LoadConfigInRemoteInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);
        }

        public static string LoadConfigInDllInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, NameConfig.LoadConfigNameInDll);
        }

        public static string LoadConfigInDllInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.LoadConfigNameInDll);
        }

        // Bundle路径
        public static string BundleInTempInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory, bundleName);
        }

        public static string BundleInLocalInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.LocalDirectory, bundleName);
        }

        public static string BundleInRemoteInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.RemoteDirectory, bundleName);
        }

        public static string BundleInDllInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.DllDirectory, bundleName);
        }
    }
}