using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth
{
    public class UpdateConfig : IProcedure
    {
        public void EnterProcedure()
        {
            // 临时下载配置的存放路径
            List<string> tempDownloadConfigInPersistentDataPathList = new List<string>()
            {
                PathConfig.TempDownloadConfigInRemoteInPersistentDataPath(),
                PathConfig.TempDownloadConfigInDllInPersistentDataPath(),
            };

            // 下载配置的存放路径
            List<string> downloadConfigInPersistentDataPathList = new List<string>()
            {
                PathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                PathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 更新下载配置
            int count = tempDownloadConfigInPersistentDataPathList.Count;
            for (int index = 0; index < count; index++)
            {
                if (File.Exists(downloadConfigInPersistentDataPathList[index]))
                {
                    File.Delete(downloadConfigInPersistentDataPathList[index]);
                }
                File.Move(tempDownloadConfigInPersistentDataPathList[index], downloadConfigInPersistentDataPathList[index]);
            }

            // 更新本地Version
            VersionConfig versionConfig = GameEntry.DownloadCore.GetVersionConfig(PathConfig.VersionInPersistentDataPath());
            if (versionConfig != null)
            {
                File.Delete(PathConfig.VersionInPersistentDataPath());
            }
            File.Move(PathConfig.TempVersionInPersistentDataPath(), PathConfig.VersionInPersistentDataPath());

            GameEntry.DownloadCore.DownloadBundleStartPos = 0;

            // 因为文件名发生了变化，所以把配置缓存清空
            GameEntry.DownloadCore.ClearConfigCache();

            ExitProcedure();
        }

        public void ExitProcedure()
        {
            new ScanDeleteBundles().EnterProcedure();
        }
    }
}
