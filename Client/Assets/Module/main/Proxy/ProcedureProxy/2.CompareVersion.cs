using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class CompareVersion : IProcedure
    {
        private readonly DownloadProxy downloadProxy;
        private readonly PathConfig pathConfig;
        
        [Inject]
        public CompareVersion(DownloadProxy downloadProxy, PathConfig pathConfig)
        {
            this.downloadProxy = downloadProxy;
            this.pathConfig = pathConfig;
        }

        async UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellationToken)
        {
            downloadProxy.Clear();

            // 请求远端版本配置
            bool result = await downloadProxy.DownloadAsync(pathConfig.VersionInServerPath(),
                pathConfig.TempVersionInPersistentDataPath(), cancellationToken);

            if (!result)
            {
                // TODO .. 弹窗提示  Y .. 重试  N .. 退出
                UnityEngine.Debug.LogError($"远端的版本配置文件无法下载!!");
                return PROCEDURE.Error;
            }

            VersionConfig baseVersionConfig = downloadProxy.GetVersionConfig(pathConfig.BaseVersionPath());
            if (baseVersionConfig == null)
            {
                // TODO .. 弹窗提示 .. 客户端资源损坏, 重新下载客户端  Y .. 退出
                UnityEngine.Debug.LogError("本地的版本配置文件不存在！！");
                return PROCEDURE.Error;
            }

            VersionConfig tempPersistentVersionConfig =
                downloadProxy.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath());
            if (tempPersistentVersionConfig == null)
            {
                // TODO .. 弹窗提示 .. 尝试重新下载  Y .. 重试
                UnityEngine.Debug.LogError("持久化目录的临时版本配置文件不存在！！");
                return PROCEDURE.Error;
            }

            if (tempPersistentVersionConfig.BaseVersion != baseVersionConfig.BaseVersion)
            {
                // TODO .. 弹窗提示 .. 客户端版本不是最新的, 重新下载客户端  Y .. 退出
                UnityEngine.Debug.LogWarning("客户端版本与资源基础版本不一致！！");
                return PROCEDURE.Error;
            }

            // 与上一次热更的版本文件对比
            VersionConfig persistentVersionConfig =
                downloadProxy.GetVersionConfig(pathConfig.VersionInPersistentDataPath());

            if (persistentVersionConfig != null
                && persistentVersionConfig.Version == tempPersistentVersionConfig.Version)
            {
                UnityEngine.Debug.Log("无需更新！！");
                File.Delete(pathConfig.TempVersionInPersistentDataPath());
                return PROCEDURE.Finish;
            }
            else
            {
                // 需要更新
                return PROCEDURE.Continue;
            }
        }
    }
}