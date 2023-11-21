using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class DownloadLoadConfig : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;

                public DownloadLoadConfig(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                    this.pathConfig = pathConfig;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    string version = downloadCore.GetVersionConfig(pathConfig.TempVersionInPersistentDataPath()).Version;

                    // 资源服务器的加载配置的存放路径
                    List<Func<string, string>> loadConfigInServerPathList = new List<Func<string, string>>()
                    {
                        version => pathConfig.LoadConfigInRemoteInServerPath(version),
                        version => pathConfig.LoadConfigInDllInServerPath(version),
                    };

                    // 加载配置的存放路径
                    List<string> loadConfigInPersistentDataPathList = new List<string>()
                    {
                        pathConfig.LoadConfigInRemoteInPersistentDataPath(),
                        pathConfig.LoadConfigInDllInPersistentDataPath(),
                    };

                    // 下载加载配置
                    int count = loadConfigInServerPathList.Count;
                    for (int index = 0; index < count; index++)
                    {
                        bool result = await downloadCore.Download(loadConfigInServerPathList[index](version), loadConfigInPersistentDataPathList[index]);
                        if (!result)
                        {
                            // TODO .. 弹窗提示 .. 资源服务器缺少加载配置  Y .. 重试
                            UnityEngine.Debug.LogError("资源服务器缺少加载配置!!");
                            return ProcedureInfo.Error;
                        }
                    }
                    return ProcedureInfo.Continue;
                }
            }
        }
    }
}

