using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public static class TasksExtensions
    {
        // Brian: 写扩展方法处理一发即忘问题
        // Brian Lagunas：https://www.youtube.com/watch?v=O1Tx-k4Vao0
        // 适用场景
        //  => 构造器
        //  => ..
        public static  async void SafeFireAndForget(this Task task, Action? onCompleted = null, Action<Exception>? onError = null)
        {
            try
            {
                await task.ConfigureAwait(false);
                onCompleted?.Invoke();
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
        }


        // public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        // {
        //     using var cts = new CancellationTokenSource();
        //     var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cts.Token));
        //     if(completedTask != task)
        //     {
        //         cts.Cancel();
        //         throw new TimeoutException();
        //     }
        //     await task;
        // }
        // public static async Task TimeoutAfter(this Task task, int millisecondsDelay)
        // {
        //     using var cts = new CancellationTokenSource();
        //     var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, cts.Token));
        //     if(completedTask != task)
        //     {
        //         cts.Cancel();
        //         throw new TimeoutException();
        //     }
        //     await task;
        // }

        // public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        // {
        //     using var cts = new CancellationTokenSource();
        //     var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cts.Token));
        //     if(completedTask != task)
        //     {
        //         cts.Cancel();
        //         throw new TimeoutException();
        //     }
        //     return await task;
        // }
        // public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int millisecondsDelay)
        // {
        //     using var cts = new CancellationTokenSource();
        //     var completedTask = await Task.WhenAny(task, Task.Delay(millisecondsDelay, cts.Token));
        //     if(completedTask != task)
        //     {
        //         cts.Cancel();
        //         throw new TimeoutException();
        //     }
        //     return await task;
        // }
    }
}
