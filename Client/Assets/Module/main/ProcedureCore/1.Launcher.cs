using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public partial class GameEntry
    {
        public partial class ProcedureCore
        {
            public class Launcher: IProcedure
            {
                private readonly AssetMode assetMode;

                public Launcher(AssetMode assetMode)
                {
                    this.assetMode = assetMode;
                }

                public async Task<ProcedureInfo> Execute()
                {
                    if (assetMode == AssetMode.RemoteAB)
                    {
                        return ProcedureInfo.Continue;
                    }
                    else
                    {
                        return ProcedureInfo.Through;
                    }
                }
            }
        }
    }
}

