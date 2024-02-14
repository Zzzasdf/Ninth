using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public class StartUp : IProcedure
    {
        async UniTask<PROCEDURE> IProcedure.StartAsync(CancellationToken cancellationToken)
        {
            UnityEngine.Debug.Log("资源更新完毕！！");
            return PROCEDURE.Continue;
        }
    }
}