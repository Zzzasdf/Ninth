using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class ScanDeleteBundles : IProcedure
            {
                private readonly DownloadCore downloadCore;
                private readonly PathConfig pathConfig;
                public ScanDeleteBundles(DownloadCore downloadCore, PathConfig pathConfig)
                {
                    this.downloadCore = downloadCore;
                    this.pathConfig = pathConfig;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    // 本地存放下载配置路径
                    List<string> downloadConfigPathList = new List<string>()
                    {
                        pathConfig.DownloadConfigInRemoteInPersistentDataPath(),
                        pathConfig.DownloadConfigInDllInPersistentDataPath(),
                    };

                    // 本地存放下载文件目录
                    List<string> downloadDirPath = new List<string>()
                    {
                        pathConfig.bundleRootInRemoteInPersistentDataPath,
                        pathConfig.bundleRootInDllInPersistentDataPath
                    };

                    for (int index = 0; index < downloadConfigPathList.Count; index++)
                    {
                        DownloadConfig downloadConfig = downloadCore.GetDownloadConfig(downloadConfigPathList[index]);
                        List<string> bundles = downloadConfig.BundleInfos.Keys.ToList();

                        // 扫描目录
                        DirectoryInfo directoryInfo = new DirectoryInfo(downloadDirPath[index]);
                        FileInfo[] fileArray = directoryInfo.GetFiles()
                            .Where(x => !x.FullName.EndsWith(".json")).ToArray();
                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            if (!bundles.Contains(fileArray[i].Name))
                            {
                                File.Delete(fileArray[i].FullName);
                            }
                        }
                    }
                    return ProcedureInfo.Continue;
                }
            }
        }
    }
}
