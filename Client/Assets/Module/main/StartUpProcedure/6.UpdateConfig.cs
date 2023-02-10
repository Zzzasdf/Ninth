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

            // 重置下载开始点
            GameEntry.DownloadCore.DownloadBundleStartPos = 0;
            ExitProcedure();
        }

        public void ExitProcedure()
        {
            new StartUp().EnterProcedure();
        }
    }
}
