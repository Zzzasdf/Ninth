using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Ninth
{
    public class UpdateConfig : IProcedure
    {
        private readonly DownloadProxy downloadProxy;
        private readonly PathConfig pathConfig;

        public UpdateConfig(DownloadProxy downloadProxy, PathConfig pathConfig)
        {
            this.downloadProxy = downloadProxy;
            this.pathConfig = pathConfig;
        }

        async UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellationToken)
        {
            // 临时下载配置的存放路径
            List<string> tempDownloadConfigInPersistentDataPathList = new List<string>()
            {
                pathConfig.TempDownloadConfigInRemoteInPersistentDataPath(),
                pathConfig.TempDownloadConfigInDllInPersistentDataPath(),
            };

            // 下载配置的存放路径
            List<string> downloadConfigInPersistentDataPathList = new List<string>()
            {
                pathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                pathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 更新下载配置
            int count = tempDownloadConfigInPersistentDataPathList.Count;
            for (int index = 0; index < count; index++)
            {
                if (File.Exists(downloadConfigInPersistentDataPathList[index]))
                {
                    File.Delete(downloadConfigInPersistentDataPathList[index]);
                }

                File.Move(tempDownloadConfigInPersistentDataPathList[index],
                    downloadConfigInPersistentDataPathList[index]);
            }

            // 更新本地Version
            VersionConfig versionConfig = downloadProxy.GetVersionConfig(pathConfig.VersionInPersistentDataPath());
            if (versionConfig != null)
            {
                File.Delete(pathConfig.VersionInPersistentDataPath());
            }

            File.Move(pathConfig.TempVersionInPersistentDataPath(), pathConfig.VersionInPersistentDataPath());

            downloadProxy.DownloadBundleStartPos = 0;

            // 因为文件名发生了变化，所以把配置缓存清空
            downloadProxy.ClearConfigCache();

            return PROCEDURE.Continue;
        }
    }
}