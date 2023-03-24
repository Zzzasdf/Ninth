using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        public static NameConfig NameConfig
        {
            get => SOCore.GetNameConfig();
        }

        // Temp
        public static string SourceDataTempPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.PackTempDirectory);
        }

        // Local
        public static string SourceDataLocalPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.LocalDirectory);
        }
        public static string CopyDataLocalPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, NameConfig.LocalDirectory);
        }

        // Remote
        public static string SourceDataRemotePathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.RemoteDirectory);
        }
        public static string CopyDataRemotePathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, NameConfig.RemoteDirectory);
        }

        // Dll
        public static string SourceDataDllPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), NameConfig.DllDirectory);
        }
        public static string CopyDataDllPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, NameConfig.DllDirectory);
        }

        // 客户端目录
        public static string PlayerSourceDirectory(string version)
        {
            return string.Format("{0}/{1}", PlayerSourceDataVersionRoot, version.ToString());
        }
    }
}