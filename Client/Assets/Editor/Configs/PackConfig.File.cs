using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        // Local(StreamingAsset) Version
        public string BaseVersion()
        {
            return pathConfig.BaseVersionPath();
        }

        // Remote Apply Version
        public string ApplyVersionInSourceDataPath()
        {
            return string.Format("{0}/{1}", BundleSourceDataVersionRoot, nameConfig.VersionConfigName);
        }

        // Version
        public string VersionInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.VersionConfigName);
        }
        public string VersionInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.VersionConfigName);
        }
        public string VersionInCopyDataPath()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.VersionConfigName);
        }


        // 下载配置
        public string DownloadConfigInRemoteInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.DownloadConfigNameInRemote);
        }

        public string DownloadConfigInRemoteInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.RemoteDirectory, nameConfig.DownloadConfigNameInRemote);
        }

        public string DownloadConfigInDllInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.DownloadConfigNameInDll);
        }

        public string DownloadConfigInDllInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DllDirectory, nameConfig.DownloadConfigNameInDll);
        }

        // 加载配置
        public string LoadConfigInLocalInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.LoadConfigNameInLocal);
        }

        public string LoadConfigInLocalInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.LocalDirectory, nameConfig.LoadConfigNameInLocal);
        }

        public string LoadConfigInRemoteInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.LoadConfigNameInRemote);
        }

        public string LoadConfigInRemoteInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.RemoteDirectory, nameConfig.LoadConfigNameInRemote);
        }

        public string LoadConfigInDllInSourceDataTempPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, nameConfig.LoadConfigNameInDll);
        }

        public string LoadConfigInDllInSourceDataPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DllDirectory, nameConfig.LoadConfigNameInDll);
        }

        // Bundle路径
        public string BundleInTempInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory, bundleName);
        }

        public string BundleInLocalInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.LocalDirectory, bundleName);
        }

        public string BundleInRemoteInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.RemoteDirectory, bundleName);
        }

        public string BundleInDllInSourceDataPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DllDirectory, bundleName);
        }
    }
}