using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class IncreaseBundles : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;

                public IncreaseBundles(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    string version = downloadCore.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath()).Version;

                    List<Func<string, string, string>> bundleServerPathList = new List<Func<string, string, string>>
                    {
                        (version, bundleName) => pathConfig.BundleInRemoteInServerPath(version, bundleName),
                        (version, bundleName) => pathConfig.BundleInDllInServerPath(version, bundleName),
                    };

                            List<Func<string, string>> bundlePersistentDataPathList = new List<Func<string, string>>
                    {
                        bundleName => pathConfig.BundleInRemoteInPersistentDataPath(bundleName),
                        bundleName => pathConfig.BundleInDllInPersistentDataPath(bundleName),
                    };

                    // 下载新增或更改bundle
                    List<BundleInfo> increaseBundleList = downloadCore.GetIncreaseBundleList.Values.ToList();

                    for (int i = 1; i < downloadCore.IncreaseTypeNodes.Count; i++)
                    {
                        int startIndex = downloadCore.IncreaseTypeNodes[i - 1];
                        int endIndex = downloadCore.IncreaseTypeNodes[i];

                        for (int index = startIndex; index < endIndex; index++)
                        {
                            string bundleName = increaseBundleList[index].BundleName;
                            long size = increaseBundleList[index].Size;
                            downloadCore.MessageBox.DownloadNext("当前的下载进度{0}%({1}/{2})", size);

                            bool result = await downloadCore.Download(bundleServerPathList[i - 1](version, bundleName), bundlePersistentDataPathList[i - 1](bundleName));
                            if (!result)
                            {
                                // TODO .. 弹窗提示 .. 下载错误  Y .. 重试（3）
                                UnityEngine.Debug.LogError($"远端文件Remote文件夹下不存在名{bundleName}的Bundle");
                                return ProcedureInfo.Error;
                            }

                            downloadCore.DownloadBundleStartPos++;
                            downloadCore.GetCompleteDownloadIncreaseBundleAmount++;
                            downloadCore.GetCompleteDownloadIncreaseBundleProgress += size;

                            downloadCore.MessageBox.CancelRefreshProgress();
                        }
                    }
                    if (downloadCore.MessageBox != null)
                    {
                        downloadCore.MessageBox.OverStatus();
                    }
                    return ProcedureInfo.Continue;
                }
            }
        }
    }
}
