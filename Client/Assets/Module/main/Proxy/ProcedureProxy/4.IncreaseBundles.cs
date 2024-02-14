using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Ninth
{
    public class IncreaseBundles : IProcedure
    {
        private readonly DownloadProxy downloadProxy;
        private readonly PathConfig pathConfig;
        
        [Inject]
        public IncreaseBundles(DownloadProxy downloadProxy, PathConfig pathConfig)
        {
            this.downloadProxy = downloadProxy;
            this.pathConfig = pathConfig;
        }

        async UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellationToken)
        {
            string version = downloadProxy.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath()).Version;

            List<Func<string, string, string>> bundleServerPathList = new List<Func<string, string, string>>
            {
                (version, bundleName) => pathConfig.BundlePathByRemoteGroup(version, bundleName),
                (version, bundleName) => pathConfig.BundlePathByDllGroup(version, bundleName),
            };

            List<Func<string, string>> bundlePersistentDataPathList = new List<Func<string, string>>
            {
                bundleName => pathConfig.BundlePathByRemoteGroup(bundleName),
                bundleName => pathConfig.BundlePathByDllGroup(bundleName),
            };

            // 下载新增或更改bundle
            List<BundleInfo> increaseBundleList = downloadProxy.GetIncreaseBundleList.Values.ToList();

            for (int i = 1; i < downloadProxy.IncreaseTypeNodes.Count; i++)
            {
                int startIndex = downloadProxy.IncreaseTypeNodes[i - 1];
                int endIndex = downloadProxy.IncreaseTypeNodes[i];

                for (int index = startIndex; index < endIndex; index++)
                {
                    string bundleName = increaseBundleList[index].BundleName;
                    long size = increaseBundleList[index].Size;
                    downloadProxy.MessageBox.DownloadNext("当前的下载进度{0}%({1}/{2})", size);

                    bool result = await downloadProxy.DownloadAsync(bundleServerPathList[i - 1](version, bundleName),
                        bundlePersistentDataPathList[i - 1](bundleName), cancellationToken);
                    if (!result)
                    {
                        // TODO .. 弹窗提示 .. 下载错误  Y .. 重试（3）
                        UnityEngine.Debug.LogError($"远端文件Remote文件夹下不存在名{bundleName}的Bundle");
                        return PROCEDURE.Error;
                    }

                    downloadProxy.DownloadBundleStartPos++;
                    downloadProxy.GetCompleteDownloadIncreaseBundleAmount++;
                    downloadProxy.GetCompleteDownloadIncreaseBundleProgress += size;

                    downloadProxy.MessageBox.CancelRefreshProgress();
                }
            }

            if (downloadProxy.MessageBox != null)
            {
                downloadProxy.MessageBox.OverStatus();
            }

            return PROCEDURE.Continue;
        }
    }
}