using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class UpdateConfig : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;
                
                public UpdateConfig(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                    this.pathConfig = pathConfig;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    // 临时下载配置的存放路径
                    List<string> tempDownloadConfigInPersistentDataPathList = new List<string>()
                    {
                        pathConfig.TempDownloadConfigInRemoteInPersistentDataPath(),
                        pathConfig.TempDownloadConfigInDllInPersistentDataPath(),
                    };

                    // 下载配置的存放路径
                    List<string> downloadConfigInPersistentDataPathList = new List<string>()
                    {
                        pathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                        pathConfig.DownloadConfigInDllInPersistentDataPath(),
                    };

                    // 更新下载配置
                    int count = tempDownloadConfigInPersistentDataPathList.Count;
                    for (int index = 0; index < count; index++)
                    {
                        if (File.Exists(downloadConfigInPersistentDataPathList[index]))
                        {
                            File.Delete(downloadConfigInPersistentDataPathList[index]);
                        }
                        File.Move(tempDownloadConfigInPersistentDataPathList[index], downloadConfigInPersistentDataPathList[index]);
                    }

                    // 更新本地Version
                    VersionConfig versionConfig = downloadCore.GetVersionConfig(pathConfig.VersionInPersistentDataPath());
                    if (versionConfig != null)
                    {
                        File.Delete(pathConfig.VersionInPersistentDataPath());
                    }
                    File.Move(pathConfig.TempVersionInPersistentDataPath(), pathConfig.VersionInPersistentDataPath());

                    downloadCore.DownloadBundleStartPos = 0;

                    // 因为文件名发生了变化，所以把配置缓存清空
                    downloadCore.ClearConfigCache();
                    
                    return ProcedureInfo.Continue;
                }
            }
        }
    }
}
