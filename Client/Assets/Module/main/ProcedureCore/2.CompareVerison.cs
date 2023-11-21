using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class CompareVerison : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;

                public CompareVerison(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                    this.pathConfig = pathConfig;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    downloadCore.Clear();

                    // 请求远端版本配置
                    bool result = await downloadCore.Download(pathConfig.VersionInServerPath(), pathConfig.TempVersionInPersistentDataPath());

                    if (!result)
                    {
                        // TODO .. 弹窗提示  Y .. 重试  N .. 退出
                        UnityEngine.Debug.LogError($"远端的版本配置文件无法下载!!");
                        return ProcedureInfo.Error;
                    }

                    VersionConfig baseVersionConfig = downloadCore.GetVersionConfig(pathConfig.BaseVersionPath());
                    if (baseVersionConfig == null)
                    {
                        // TODO .. 弹窗提示 .. 客户端资源损坏, 重新下载客户端  Y .. 退出
                        UnityEngine.Debug.LogError("本地的版本配置文件不存在！！");
                        return ProcedureInfo.Error;
                    }

                    VersionConfig tempPersistentVersionConfig = downloadCore.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath());
                    if (tempPersistentVersionConfig == null)
                    {
                        // TODO .. 弹窗提示 .. 尝试重新下载  Y .. 重试
                        UnityEngine.Debug.LogError("持久化目录的临时版本配置文件不存在！！");
                        return ProcedureInfo.Error;
                    }
                    if (tempPersistentVersionConfig.BaseVersion != baseVersionConfig.BaseVersion)
                    {
                        // TODO .. 弹窗提示 .. 客户端版本不是最新的, 重新下载客户端  Y .. 退出
                        UnityEngine.Debug.LogWarning("客户端版本与资源基础版本不一致！！");
                        return ProcedureInfo.Error;
                    }

                    // 与上一次热更的版本文件对比
                    VersionConfig persistentVersionConfig = downloadCore.GetVersionConfig(pathConfig.VersionInPersistentDataPath());

                    if (persistentVersionConfig != null
                        && persistentVersionConfig.Version == tempPersistentVersionConfig.Version)
                    {
                        UnityEngine.Debug.Log("无需更新！！");
                        File.Delete(pathConfig.TempVersionInPersistentDataPath());
                        return ProcedureInfo.Through;
                    }
                    else
                    {
                        // 需要更新
                        return ProcedureInfo.Continue;
                    }
                }
            }
        }
    }
}

