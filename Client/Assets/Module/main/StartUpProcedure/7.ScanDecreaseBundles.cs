using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Ninth
{
    public class ScanDeleteBundles : IProcedure
    {
        public void EnterProcedure()
        {
            // 本地存放下载配置路径
            List<string> downloadConfigPathList = new List<string>()
            {
                PathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                PathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 本地存放下载文件目录
            List<string> downloadDirPath = new List<string>()
            {
                PathConfig.BundleRootInRemoteInPersistentDataPath,
                PathConfig.BundleRootInDllInPersistentDataPath
            };

            for (int index = 0; index < downloadConfigPathList.Count; index++)
            {
                DownloadConfig downloadConfig = GameEntry.DownloadCore.GetDownloadConfig(downloadConfigPathList[index]);
                List<string> bundles = downloadConfig.BundleInfos.Keys.ToList();

                // 扫描目录
                DirectoryInfo directoryInfo = new DirectoryInfo(downloadDirPath[index]);
                FileInfo[] fileArray = directoryInfo.GetFiles()
                    .Where(x => !x.FullName.EndsWith(".json")).ToArray();
                for(int i = 0; i < fileArray.Length; i++)
                {
                    if (!bundles.Contains(fileArray[i].Name))
                    {
                        File.Delete(fileArray[i].FullName);
                    }
                }
            }
            ExitProcedure();
        }

        public void ExitProcedure()
        {
            new StartUp().EnterProcedure();
        }
    }
}
