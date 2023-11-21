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
            public class StartUp : IProcedure
            {
                public async Task<ProcedureInfo> Execute()
                {
                    UnityEngine.Debug.Log("资源更新完毕！！");
                    return ProcedureInfo.Continue;
                }
            }
        }
    }
}
