using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public class StartUp : IProcedure
    {
        public void EnterProcedure()
        {
            ExitProcedure();
        }

        public void ExitProcedure()
        {
            UnityEngine.Debug.Log("资源更新完毕！！");
            new LoadDll().EnterProcedure();
        }
    }
}
