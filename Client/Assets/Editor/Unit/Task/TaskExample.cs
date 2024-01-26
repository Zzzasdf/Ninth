using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Ninth.HotUpdate;

namespace Ninth.Editor
{
    // 1、多线程与异步是不同的概念
    //  => 异步并不意味着多线程，单线程同样可以异步（比如可以借助CPU的时间片轮转调度之类的）
    //  => 异步默认借助线程池
    //  => 多线程经常阻塞，而异步要求不阻塞
    // 总结
    //  => 多线程：多个人做多件事
    //  => 异步：一个人做多件事

    // 2、适用场景
    //  => 适合 IO 密集型操作（访问API，读取本地文件之类的）
    //  => 适合短暂的小任务
    //  => 避免线程阻塞，提高系统响应能力
    
    // 3、对于异步任务的抽象
    //  => 开启异步任务后，当前线程并不会阻塞，而是可以去做其他事情
    //  => 异步任务（默认）会借助线程池在其他线程上运行
    //  => 获取结果后回到之前的状态

    // 4、异步方法
    //  => await 一个方法不一定是一个 async Task 方法
    //  => 标记 async 只是为了在方法里使用 await
    //  => await 关键字会等待异步任务的结束，并获得结果
    //  => async + await 会将方法包装成状态机，await 类似于检查点
    //    => MoveNext 方法会在底层调用，从而切换状态
    //  => async Task
    //    => 
    //  => async void

    // eg！另有 ValueTask 值类型版本
    public class TaskExample
    {
        // 异步任务的各种状态
        // 正在运行、完成、结果、报错等
        [Test]
        public void Status()
        {
            var task = new Task<string>(() =>
            {
                Thread.Sleep(1500);
                return "done";
            });
            task.Status.Log(); // Created
            task.Start();
            task.Status.Log(); // WaitingToRun
            Thread.Sleep(1000);
            task.Status.Log(); // Runing
            Thread.Sleep(2000);
            task.Status.Log(); // RanToCompletion
            task.Result.Log("Result => {0}");
        }

        // 线程切换
        // await Task方法默认的返回原线程
        // ConfigureAwait(false) 只会影响 FooAsync() 下文的线程，该方法返回后，不影响 ThreadChange() 线程
        // 只有在异步方法中配置了 ConfigureAwait(false) 才会影响下文的线程，与 await 返回的线程无关
        [Test]
        public async void ThreadChange()
        {
            "Before".Log("Main {0}");
            await FooAsync();
            "After".Log("Main {0}");

            async Task FooAsync()
            {
                "Before".Log("Async {0}");
                await Task.Delay(1000).ConfigureAwait(false);
                "After".Log("Async {0}");
            }
        }
    }
}