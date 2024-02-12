using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;


namespace Ninth
{
    public class CompareDownloadConfig : IProcedure
    {
        private readonly DownloadProxy downloadProxy;
        private readonly PathConfig pathConfig;
            
        [Inject]
        public CompareDownloadConfig(DownloadProxy downloadProxy, PathConfig pathConfig)
        {
            this.downloadProxy = downloadProxy;
            this.pathConfig = pathConfig;
        }

        async UniTask<ProcedureInfo> IProcedure.StartAsync(CancellationToken cancellationToken = default)
        {
            long totalSize = 0;

            // 节点初始化
            downloadProxy.IncreaseTypeNodes = new List<int>();
            downloadProxy.IncreaseTypeNodes.Add(0);

            // 当前版本
            string version = downloadProxy.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath()).Version;

            // 资源服务器存放下载配置路径
            List<string> serverDownloadConfigPathList = new List<string>()
            {
                pathConfig.DownloadConfigInRemoteInServerPath(version),
                pathConfig.DownloadConfigInDllInServerPath(version),
            };

            // 临时下载配置存放路径
            List<string> tempDownloadConfigPathList = new List<string>()
            {
                pathConfig.TempDownloadConfigInRemoteInPersistentDataPath(),
                pathConfig.TempDownloadConfigInDllInPersistentDataPath(),
            };

            // 本地存放下载配置路径
            List<string> downloadConfigPathList = new List<string>()
            {
                pathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                pathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 下载下载配置
            int count = serverDownloadConfigPathList.Count;
            for (int index = 0; index < count; index++)
            {
                string serverDownloadConfigPath = serverDownloadConfigPathList[index];
                string tempDownloadConfigPath = tempDownloadConfigPathList[index];

                bool result = await downloadProxy.Download(serverDownloadConfigPath, tempDownloadConfigPath, cancellationToken);
                if (!result)
                {
                    // TODO .. 弹窗提示 .. 无法获取到最新的下载配置, 重新下载  Y .. 重试  N .. 退出  
                    UnityEngine.Debug.LogError("从资源服务器下载下载配置出错");
                    return ProcedureInfo.Error;
                }
            }

            int bundleIndex = 0;
            int bundleBreakPos = downloadProxy.GetDownloadBundleStartPos();

            for (int index = 0; index < count; index++)
            {
                DownloadConfig tempDownloadConfig = downloadProxy.GetDownloadConfig(tempDownloadConfigPathList[index]);
                DownloadConfig downloadConfig = downloadProxy.GetDownloadConfig(downloadConfigPathList[index]);

                // 增加bundle列表
                foreach (var item in tempDownloadConfig.BundleInfos)
                {
                    string bundleName = item.Key;
                    BundleInfo bundleInfo = item.Value;

                    // 第一次下载这个下载配置 || 本地第一次下载这个bundle
                    if (downloadConfig == null ||
                        !downloadConfig.BundleInfos.TryGetValue(bundleName, out BundleInfo value))
                    {
                        // 断点续传，忽略下载成功的bundle
                        if (bundleIndex < bundleBreakPos)
                        {
                            bundleIndex++;
                            continue;
                        }

                        downloadProxy.IncreaseBundle(bundleName, bundleInfo);
                        totalSize += bundleInfo.Size;
                    }
                    else
                    {
                        if (value.Crc != bundleInfo.Crc)
                        {
                            // 断点续传，忽略下载成功的bundle
                            if (bundleIndex < bundleBreakPos)
                            {
                                bundleIndex++;
                                continue;
                            }

                            downloadProxy.IncreaseBundle(bundleName, bundleInfo);
                            totalSize += bundleInfo.Size;
                        }
                    }
                }

                downloadProxy.IncreaseTypeNodes.Add(downloadProxy.GetIncreaseBundleList.Count);
            }

            downloadProxy.GetTotalIncreaseBundleSize = totalSize;
            if (downloadProxy.GetTotalIncreaseBundleSize == 0)
            {
                return ProcedureInfo.Continue;
            }
            else
            {
                downloadProxy.MessageBox = new MessageBox(downloadProxy);

                // 弹窗
                return await downloadProxy.MessageBox.DownloadBeforce(
                    $"发现新的版本，需要下载大小为：{downloadProxy.SizeToString(totalSize)}",
                    "确认下载",
                    () => ProcedureInfo.Continue,
                    "取消",
                    () =>
                    {
                        Application.Quit();
                        return ProcedureInfo.Error;
                    });
            }
        }
    }
}