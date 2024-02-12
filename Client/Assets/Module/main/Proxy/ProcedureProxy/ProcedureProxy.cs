using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public enum ProcedureInfo
    {
        Error, // 报错不执行
        Continue, // 继续运行下一步
        Through, // 通过验证，直接运行启动流程
    }
    
    public class ProcedureProxy: IProcedureProxy, IAsyncStartable
    {
        private readonly IDownloadProxy downloadProxy;
        private readonly IObjectResolver objectResolver;
        
        [Inject]
        public ProcedureProxy(IDownloadProxy downloadProxy, IObjectResolver objectResolver)
        {
            this.downloadProxy = downloadProxy;
            this.objectResolver = objectResolver;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var procedures = new List<IProcedure>()
            {
                objectResolver.Resolve<Launcher>(),
                objectResolver.Resolve<CompareVersion>(),
                objectResolver.Resolve<CompareDownloadConfig>(),
                objectResolver.Resolve<IncreaseBundles>(),
                objectResolver.Resolve<DownloadLoadConfig>(),
                objectResolver.Resolve<UpdateConfig>(),
                objectResolver.Resolve<ScanDecreaseBundles>(),
                objectResolver.Resolve<StartUp>(),
                objectResolver.Resolve<LoadDll>(),
            };
            foreach (var procedure in procedures)
            {
                ProcedureInfo info = await procedure.StartAsync();
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
                        await procedures[^1].StartAsync();
                        break;
                    }
                }
            }
        }
    }
}