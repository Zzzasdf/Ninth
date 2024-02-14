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
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DirectoryTempNameByPack);
        }

        // Local
        public string SourceDataLocalPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DirectoryNameByLocalGroup);
        }
        public string CopyDataLocalPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.DirectoryNameByLocalGroup);
        }

        // Remote
        public string SourceDataRemotePathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DirectoryNameByRemoteGroup);
        }
        public string CopyDataRemotePathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.DirectoryNameByRemoteGroup);
        }

        // Dll
        public string SourceDataDllPathDirectory(string version)
        {
            return string.Format("{0}/{1}/{2}", BundleSourceDataVersionRoot, version.ToString(), nameConfig.DirectoryNameByDllGroup);
        }
        public string CopyDataDllPathDirectory()
        {
            return string.Format("{0}/{1}", CopyDataRoot, nameConfig.DirectoryNameByDllGroup);
        }

        // 客户端目录
        public string PlayerSourceDirectory(string version)
        {
            return string.Format("{0}/{1}", PlayerSourceDataVersionRoot, version.ToString());
        }
    }
}