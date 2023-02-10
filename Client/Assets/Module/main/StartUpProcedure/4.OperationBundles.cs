using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

namespace Ninth
{
    public class OperationBundles : IProcedure
    {
        public async void EnterProcedure()
        {
            long version = GameEntry.DownloadCore.GetVersionConfig(PathConfig.TempVersionInPersistentDataPath()).Version;

            List<Func<long, string, string>> bundleServerPathList = new List<Func<long, string, string>>
            {
                (version, bundleName) => PathConfig.BundleInRemoteInServerPath(version, bundleName),
                (version, bundleName) => PathConfig.BundleInDllInServerPath(version, bundleName),
            };

            List<Func<string, string>> bundlePersistentDataPathList = new List<Func<string, string>>
            {
                bundleName => PathConfig.BundleInRemoteInPersistentDataPath(bundleName),
                bundleName => PathConfig.BundleInDllInPersistentDataPath(bundleName),
            };

            // 下载新增或更改bundle
            List<BundleInfo> increaseBundleList = GameEntry.DownloadCore.GetIncreaseBundleList.Values.ToList();

            for (int i = 1; i < GameEntry.DownloadCore.IncreaseTypeNodes.Count; i++)
            {
                int startIndex = GameEntry.DownloadCore.IncreaseTypeNodes[i - 1];
                int endIndex = GameEntry.DownloadCore.IncreaseTypeNodes[i];

                for (int index = startIndex; index < endIndex; index++)
                {
                    string bundleName = increaseBundleList[index].BundleName;
                    long size = increaseBundleList[index].Size;
                    GameEntry.DownloadCore.MessageBox.DownloadNext("当前的下载进度{0}%({1}/{2})", size);
                    
                    bool result = await GameEntry.DownloadCore.Download(bundleServerPathList[i](version, bundleName), bundlePersistentDataPathList[i](bundleName));
                    if (!result)
                    {
                        // TODO .. 弹窗提示 .. 下载错误  Y .. 重试（3）
                        UnityEngine.Debug.LogError($"远端文件Remote文件夹下不存在名{bundleName}的Bundle");
                        return;
                    }

                    GameEntry.DownloadCore.DownloadBundleStartPos++;
                    GameEntry.DownloadCore.GetCompleteDownloadIncreaseBundleAmount++;
                    GameEntry.DownloadCore.GetCompleteDownloadIncreaseBundleProgress += size;

                    GameEntry.DownloadCore.MessageBox.CancelRefreshProgress();
                }
            }
            if (GameEntry.DownloadCore.MessageBox != null)
            {
                GameEntry.DownloadCore.MessageBox.OverStatus();
            }

            // 删除废弃Bundle
            List<BundleInfo> decreaseBundleList = GameEntry.DownloadCore.GetDecreaseBundleList.Values.ToList();

            for (int i = 1; i < GameEntry.DownloadCore.DecreaseTypeNodes.Count; i++)
            {
                int startIndex = GameEntry.DownloadCore.DecreaseTypeNodes[i - 1];
                int endIndex = GameEntry.DownloadCore.DecreaseTypeNodes[i];

                for (int index = startIndex; index < endIndex; index++)
                {
                    string bundleName = decreaseBundleList[index].BundleName;
                    string path = bundlePersistentDataPathList[i](bundleName);
                    File.Delete(path);
                }
            }
            ExitProcedure();
        }

        public void ExitProcedure()
        {
            new DownloadLoadConfig().EnterProcedure();
        }
    }

}
