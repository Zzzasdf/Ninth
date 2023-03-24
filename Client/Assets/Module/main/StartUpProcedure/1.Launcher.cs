using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public class Launcher: IProcedure
    {
        public void EnterProcedure()
        {
            if (SOCore.GetGlobalConfig().AssetMode == AssetMode.RemoteAB)
            {
                ExitProcedure();
            }
            else
            {
                new StartUp().EnterProcedure();
            }
        }

        public void ExitProcedure()
        {
            new CompareVerison().EnterProcedure();
        }
    }
}

