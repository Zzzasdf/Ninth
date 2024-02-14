using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public class ScanDecreaseBundles : IProcedure
    {
        private readonly DownloadProxy downloadProxy;
        private readonly PathConfig pathConfig;

        public ScanDecreaseBundles(DownloadProxy downloadProxy, PathConfig pathConfig)
        {
            this.downloadProxy = downloadProxy;
            this.pathConfig = pathConfig;
        }

        async UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellationToken)
        {
            // 本地存放下载配置路径
            List<string> downloadConfigPathList = new List<string>()
            {
                pathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                pathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 本地存放下载文件目录
            List<string> downloadDirPath = new List<string>()
            {
                pathConfig.bundleRootPath_RemoteGroup_PersistentData,
                pathConfig.bundleRootPath_DllGroup_PersistentData
            };

            for (int index = 0; index < downloadConfigPathList.Count; index++)
            {
                DownloadConfig downloadConfig = downloadProxy.GetDownloadConfig(downloadConfigPathList[index]);
                List<string> bundles = downloadConfig.BundleInfos.Keys.ToList();

                // 扫描目录
                DirectoryInfo directoryInfo = new DirectoryInfo(downloadDirPath[index]);
                FileInfo[] fileArray = directoryInfo.GetFiles()
                    .Where(x => !x.FullName.EndsWith(".json")).ToArray();
                for (int i = 0; i < fileArray.Length; i++)
                {
                    if (!bundles.Contains(fileArray[i].Name))
                    {
                        File.Delete(fileArray[i].FullName);
                    }
                }
            }

            return PROCEDURE.Continue;
        }
    }
}