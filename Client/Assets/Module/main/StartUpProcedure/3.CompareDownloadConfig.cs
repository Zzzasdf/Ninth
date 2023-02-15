using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Ninth
{
    public class CompareDownloadConfig : IProcedure
    {
        public async void EnterProcedure()
        {
            long totalSize = 0;

            // 节点初始化
            GameEntry.DownloadCore.IncreaseTypeNodes = new List<int>();
            GameEntry.DownloadCore.IncreaseTypeNodes.Add(0);

            // 当前版本
            string version = GameEntry.DownloadCore.GetVersionConfig(PathConfig.TempVersionInPersistentDataPath()).Version;

            // 资源服务器存放下载配置路径
            List<string> serverDownloadConfigPathList = new List<string>()
            {
                PathConfig.DownloadConfigInRemoteInServerPath(version),
                PathConfig.DownloadConfigInDllInServerPath(version),
            };

            // 临时下载配置存放路径
            List<string> tempDownloadConfigPathList = new List<string>()
            {
                PathConfig.TempDownloadConfigInRemoteInPersistentDataPath(),
                PathConfig.TempDownloadConfigInDllInPersistentDataPath(),
            };

            // 本地存放下载配置路径
            List<string> downloadConfigPathList = new List<string>()
            {
                PathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                PathConfig.DownloadConfigInDllInPersistentDataPath(),
            };

            // 下载下载配置
            int count = serverDownloadConfigPathList.Count;
            for (int index = 0; index < count; index++)
            {
                string serverDownloadConfigPath = serverDownloadConfigPathList[index];
                string tempDownloadConfigPath = tempDownloadConfigPathList[index];

                bool result = await GameEntry.DownloadCore.Download(serverDownloadConfigPath, tempDownloadConfigPath);
                if (!result)
                {
                    // TODO .. 弹窗提示 .. 无法获取到最新的下载配置, 重新下载  Y .. 重试  N .. 退出  
                    UnityEngine.Debug.LogError("从资源服务器下载下载配置出错");
                    return;
                }
            }

            int bundleIndex = 0;
            int bundleBreakPos = GameEntry.DownloadCore.GetDownloadBundleStartPos();

            for (int index = 0; index < count; index++)
            {
                DownloadConfig tempDownloadConfig = GameEntry.DownloadCore.GetDownloadConfig(tempDownloadConfigPathList[index]);
                DownloadConfig downloadConfig = GameEntry.DownloadCore.GetDownloadConfig(downloadConfigPathList[index]);

                // 增加bundle列表
                foreach (var item in tempDownloadConfig.BundleInfos)
                {
                    string bundleName = item.Key;
                    BundleInfo bundleInfo = item.Value;

                    // 第一次下载这个下载配置 || 本地第一次下载这个bundle
                    if (downloadConfig == null || !downloadConfig.BundleInfos.TryGetValue(bundleName, out BundleInfo value))
                    {
                        // 断点续传，忽略下载成功的bundle
                        if (bundleIndex < bundleBreakPos)
                        {
                            bundleIndex++;
                            continue;
                        }
                        GameEntry.DownloadCore.IncreaseBundle(bundleName, bundleInfo);
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
                            GameEntry.DownloadCore.IncreaseBundle(bundleName, bundleInfo);
                            totalSize += bundleInfo.Size;
                        }
                    }
                }
                GameEntry.DownloadCore.IncreaseTypeNodes.Add(GameEntry.DownloadCore.GetIncreaseBundleList.Count);
            }

            GameEntry.DownloadCore.GetTotalIncreaseBundleSize = totalSize;
            if (GameEntry.DownloadCore.GetTotalIncreaseBundleSize == 0)
            {
                ExitProcedure();
            }
            else
            {
                GameEntry.DownloadCore.MessageBox = new MessageBox();

                // 弹窗
                GameEntry.DownloadCore.MessageBox.DownloadBeforce($"发现新的版本，需要下载大小为：{GameEntry.DownloadCore.SizeToString(totalSize)}",
                    (() => ExitProcedure(), "确认下载"),
                    (() => Application.Quit(), "取消"));
            }
        }

        public void ExitProcedure()
        {
            new IncreaseBundles().EnterProcedure();
        }
    }
}