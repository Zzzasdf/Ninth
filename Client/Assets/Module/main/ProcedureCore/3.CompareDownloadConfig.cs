using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class CompareDownloadConfig : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;

                public CompareDownloadConfig(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                    this.pathConfig = pathConfig;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    long totalSize = 0;

                    // 节点初始化
                    downloadCore.IncreaseTypeNodes = new List<int>();
                    downloadCore.IncreaseTypeNodes.Add(0);

                    // 当前版本
                    string version = downloadCore.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath()).Version;

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

                        bool result = await downloadCore.Download(serverDownloadConfigPath, tempDownloadConfigPath);
                        if (!result)
                        {
                            // TODO .. 弹窗提示 .. 无法获取到最新的下载配置, 重新下载  Y .. 重试  N .. 退出  
                            UnityEngine.Debug.LogError("从资源服务器下载下载配置出错");
                            return ProcedureInfo.Error;
                        }
                    }

                    int bundleIndex = 0;
                    int bundleBreakPos = downloadCore.GetDownloadBundleStartPos();

                    for (int index = 0; index < count; index++)
                    {
                        DownloadConfig tempDownloadConfig = downloadCore.GetDownloadConfig(tempDownloadConfigPathList[index]);
                        DownloadConfig downloadConfig = downloadCore.GetDownloadConfig(downloadConfigPathList[index]);

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
                                downloadCore.IncreaseBundle(bundleName, bundleInfo);
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
                                    downloadCore.IncreaseBundle(bundleName, bundleInfo);
                                    totalSize += bundleInfo.Size;
                                }
                            }
                        }
                        downloadCore.IncreaseTypeNodes.Add(downloadCore.GetIncreaseBundleList.Count);
                    }

                    downloadCore.GetTotalIncreaseBundleSize = totalSize;
                    if (downloadCore.GetTotalIncreaseBundleSize == 0)
                    {
                        return ProcedureInfo.Continue;
                    }
                    else
                    {
                        downloadCore.MessageBox = new MessageBox(downloadCore);
                        
                        // 弹窗
                        return await downloadCore.MessageBox.DownloadBeforce($"发现新的版本，需要下载大小为：{downloadCore.SizeToString(totalSize)}",
                            "确认下载", 
                            () => ProcedureInfo.Continue,
                            "取消",
                            () => { Application.Quit(); return ProcedureInfo.Error; });
                    }
                }
            }
        }
    }
}