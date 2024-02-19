using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UnityEngine.UI
{
    public static class ButtonExtensions
    {
        // public static async UniTaskVoid AddListenerAsync(this Button? button, Func<CancellationToken, UniTask> uniTaskCall, CancellationToken cancellationToken = default)
        // {
        //     if (button == null)
        //     {
        //         return;
        //     }
        //     var btnToken = button.GetCancellationTokenOnDestroy();
        //     using var linkedCancelToken = CancellationTokenSource.CreateLinkedTokenSource(btnToken, cancellationToken);
        //     var asyncEnumerable = button.OnClickAsAsyncEnumerable();
        //     await asyncEnumerable.ForEachAwaitAsync(async _ =>
        //     {
        //         await uniTaskCall.Invoke(cancellationToken);
        //     }, cancellationToken);
        // }
        //
        // public static async UniTaskVoid AddListenerQueueAsync(this Button? button, Func<CancellationToken, UniTask> uniTaskCall, CancellationToken cancellationToken = default)
        // {
        //     if (button == null)
        //     {
        //         return;
        //     }
        //     var btnToken = button.GetCancellationTokenOnDestroy();
        //     using var linkedCancelToken = CancellationTokenSource.CreateLinkedTokenSource(btnToken, cancellationToken);
        //     var asyncEnumerable = button.OnClickAsAsyncEnumerable();
        //     await asyncEnumerable.Queue().ForEachAwaitAsync(async ca =>
        //     {
        //         await uniTaskCall.Invoke(cancellationToken);
        //     }, cancellationToken);
        // }
    }
}
