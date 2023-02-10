using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth
{
    public class CompareVerison : IProcedure
    {
        public async void EnterProcedure()
        {
            GameEntry.DownloadCore.Clear();

            // 请求远端版本配置
            bool result = await GameEntry.DownloadCore.Download(PathConfig.VersionInServerPath(), PathConfig.TempVersionInPersistentDataPath());

            if (!result)
            {
                // TODO .. 弹窗提示  Y .. 重试  N .. 退出
                UnityEngine.Debug.LogError($"远端的版本配置文件无法下载!!");
                return;
            }

            VersionConfig baseVersionConfig = GameEntry.DownloadCore.GetVersionConfig(PathConfig.BaseVersionPath());
            if (baseVersionConfig == null)
            {
                // TODO .. 弹窗提示 .. 客户端资源损坏, 重新下载客户端  Y .. 退出
                UnityEngine.Debug.LogError("本地的版本配置文件不存在！！");
                return;
            }

            VersionConfig tempPersistentVersionConfig = GameEntry.DownloadCore.GetVersionConfig(PathConfig.TempVersionInPersistentDataPath());
            if (tempPersistentVersionConfig == null)
            {
                // TODO .. 弹窗提示 .. 尝试重新下载  Y .. 重试
                UnityEngine.Debug.LogError("持久化目录的临时版本配置文件不存在！！");
                return;
            }
            if (tempPersistentVersionConfig.BaseVersion != baseVersionConfig.BaseVersion)
            {
                // TODO .. 弹窗提示 .. 客户端版本不是最新的, 重新下载客户端  Y .. 退出
                UnityEngine.Debug.LogWarning("客户端版本与资源基础版本不一致！！");
                return;
            }

            // 与上一次热更的版本文件对比
            VersionConfig persistentVersionConfig = GameEntry.DownloadCore.GetVersionConfig(PathConfig.VersionInPersistentDataPath());

            if (persistentVersionConfig != null
                && persistentVersionConfig.Version == tempPersistentVersionConfig.Version)
            {
                UnityEngine.Debug.Log("无需更新！！");
                File.Delete(PathConfig.TempVersionInPersistentDataPath());
                new StartUp().EnterProcedure();
                return;
            }
            else
            {
                // 需要更新
                ExitProcedure();
            }
        }

        public void ExitProcedure()
        {
            new CompareDownloadConfig().EnterProcedure();
        }
    }
}

