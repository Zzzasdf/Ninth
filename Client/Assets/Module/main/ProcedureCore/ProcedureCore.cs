using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth
{
    public partial class GameEntry
    {
        public sealed partial class ProcedureCore
        {
            public enum ProcedureInfo
            {
                Error, // 报错不执行
                Continue, // 继续运行下一步
                Through, // 通过验证，直接运行启动流程
            }

            private AssetConfig assetConfig;
            private PathConfig pathConfig;
            private DownloadCore downloadCore;

            public ProcedureCore(AssetConfig assetConfig, PathConfig pathConfig, DownloadCore downloadCore)
            {
                this.assetConfig = assetConfig;
                this.pathConfig = pathConfig;
                this.downloadCore = downloadCore;
            }

            public async void Start()
            {
                List<IProcedure> procedures = new List<IProcedure>();
                procedures.Add(new Launcher(assetConfig.AssetMode));
                procedures.Add(new CompareVerison(downloadCore, pathConfig));
                procedures.Add(new CompareDownloadConfig(downloadCore, pathConfig));
                procedures.Add(new IncreaseBundles(downloadCore, pathConfig));
                procedures.Add(new DownloadLoadConfig(downloadCore, pathConfig));
                procedures.Add(new UpdateConfig(downloadCore, pathConfig));
                procedures.Add(new ScanDeleteBundles(downloadCore, pathConfig));
                procedures.Add(new StartUp());
                procedures.Add(new LoadDll(assetConfig, pathConfig));

                for(int i = 0; i < procedures.Count; i++)
                {
                    ProcedureInfo info = await procedures[i].Execute();
                    switch (info)
                    {
                        case ProcedureInfo.Continue:
                        {
                            continue;
                        }
                        case ProcedureInfo.Error:
                        {
                            break;
                        }
                        case ProcedureInfo.Through:
                        {
                            await procedures[procedures.Count - 1].Execute();
                            break;
                        }
                    }
                }
            }
        }
    }
}
