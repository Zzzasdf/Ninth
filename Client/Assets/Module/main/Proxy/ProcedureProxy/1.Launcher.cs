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
        private readonly IAssetConfig assetConfig;
        
        [Inject]
        public Launcher(IAssetConfig assetConfig)
        {
            this.assetConfig = assetConfig;
        }

        UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellation)
        {
            return assetConfig.RuntimeEnv() switch
            {
                Environment.RemoteAb => UniTask.FromResult(PROCEDURE.Continue),
                _ => UniTask.FromResult(PROCEDURE.Finish)
            };
        }
    }
}