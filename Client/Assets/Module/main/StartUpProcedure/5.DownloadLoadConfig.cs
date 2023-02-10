using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth
{
    public class DownloadLoadConfig : IProcedure
    {
        public async void EnterProcedure()
        {
            long version = GameEntry.DownloadCore.GetVersionConfig(PathConfig.TempVersionInPersistentDataPath()).Version;

            // 资源服务器的加载配置的存放路径
            List<Func<long, string>> loadConfigInServerPathList = new List<Func<long, string>>()
            {
                version => PathConfig.LoadConfigInRemoteInServerPath(version),
                version => PathConfig.LoadConfigInDllInServerPath(version),
            };

            // 加载配置的存放路径
            List<string> loadConfigInPersistentDataPathList = new List<string>()
            {
                PathConfig.LoadConfigInRemoteInPersistentDataPath(),
                PathConfig.LoadConfigInDllInPersistentDataPath(),
            };

            // 下载加载配置
            int count = loadConfigInServerPathList.Count;
            for (int index = 0; index < count; index++)
            {
                bool result = await GameEntry.DownloadCore.Download(loadConfigInServerPathList[index](version), loadConfigInPersistentDataPathList[index]);
                if (!result)
                {
                    // TODO .. 弹窗提示 .. 资源服务器缺少加载配置  Y .. 重试
                    UnityEngine.Debug.LogError("资源服务器缺少加载配置!!");
                    return;
                }
            }
            ExitProcedure();
        }

        public void ExitProcedure()
        {
            new UpdateConfig().EnterProcedure();
        }
    }
}

