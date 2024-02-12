using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class Launcher: IProcedure
    {
        private readonly RuntimeEnv runtimeEnv;
        
        [Inject]
        public Launcher(AssetConfig assetConfig)
        {
            this.runtimeEnv = assetConfig.RuntimeEnv;
        }

        UniTask<ProcedureInfo> IProcedure.StartAsync(CancellationToken cancellation = default)
        {
            return runtimeEnv switch
            {
                RuntimeEnv.RemoteAb => UniTask.FromResult(ProcedureInfo.Continue),
                _ => UniTask.FromResult(ProcedureInfo.Through)
            };
        }
    }
}