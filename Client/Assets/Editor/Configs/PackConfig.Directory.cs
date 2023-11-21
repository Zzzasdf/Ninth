using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class PackConfig
    {
        // Temp
        public string SourceDataTempPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.PackTempDirectory);
        }

        // Local
        public string SourceDataLocalPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.LocalDirectory);
        }
        public string CopyDataLocalPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.LocalDirectory);
        }

        // Remote
        public string SourceDataRemotePathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.RemoteDirectory);
        }
        public string CopyDataRemotePathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.RemoteDirectory);
        }

        // Dll
        public string SourceDataDllPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DllDirectory);
        }
        public string CopyDataDllPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.DllDirectory);
        }

        // 客户端目录
        public string PlayerSourceDirectory(string version)
        {
            return string.Format("{0}/{1}", PlayerSourceDataVersionRoot, version.ToString());
        }
    }
}