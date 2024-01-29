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
    //    => 返回值依旧是 Task 类型，但是在其中可以使用 await 关键字
    //    => 在其中写返回值可以直接写 Task<T> 中的 T 类型，不用包装成 Task<T>
    //  => async void (无法等待)
    //    => 同样是状态机，但缺少记录状态的 Task 对象
    //    => 无法聚合异常 （Aggregate Exception），需要谨慎处理异常
    //    => 几乎只用于对于事件的注册
    //  => 异步编程具有传染性 (Contagious)
    //    => 一处 async ，处处 async
    //    => 几乎所有自带方法都提供了异步的版本

    // 5、重要思想：不阻塞！
    //  => await 会暂时释放当前线程，使得该线程可以执行其他工作，而不必阻塞线程直到异步操作完成
    //  => 不要在异步方法里用任何方法阻塞当前线程
    //  => 常见阻塞情形
    //    => Task.Wait() & Task.Result
    //      => 如果任务没有完成，则会阻塞当前线程，容易导致死锁
    //      => Task.GetAwaiter().GetResult()
    //          => 不会将 Exception 包装为 AggregateException
    //    => Task.Delay() vs Thread.Sleep()
    //      => 后者会阻塞当前的线程，这与异步编程的理念不符
    //      => 前者是一个异步任务，会立刻释放当前的线程
    //    => IO 等操作的同步方法
    //    => 其他繁重且耗时的任务

    // 6、同步上下文
    //  => 一种管理和协调线程的机制，允许开发者将代码的执行切换到特定的线程
    //  => WinForms 、WPF 和 Unity 拥有同步上下文（UI线程），而控制台程序默认没有
    //  => ConfigureAwait(false)
    //    => 配置任务通过 await 方法结束后是否会到原来的线程，默认为 true
    //    => 一般只有 UI 线程会采用这种策略
    //  => TaskScheduler



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
        // unity会同步上下文 await Task方法默认的返回原线程
        // ConfigureAwait(false) 只会影响 FooAsync() 下文的线程，该方法返回后，不影响 ThreadChange() 线程
        // 只有在异步方法中配置了 ConfigureAwait(false) 才会影响下文的线程，与 await 返回的线程无关
        [Test]
        public async void ThreadChangeAsync()
        {
            "Before".Log("Main {0}");
            await FooAsync();
            "After".Log("Main {0}");

            async Task FooAsync()
            {
                "Before".Log("Async {0}");
                await Task.Delay(2000).ConfigureAwait(false); // 关闭同步上下文，不会回到原线程
                (await GetValueAsync()).Log();
                "After".Log("Async {0}");
            }

            async Task<int> GetValueAsync()
            {
                await Task.Delay(2000);
                return 42;
            }
        }

        // Task 同步方法写法
        [Test]
        public void TaskSync()
        {
            async Task Foo1()
            {
                return;
            }
            Task? Foo2()
            {
                return null;
            }

            async Task<int> Foo3Async()
            {
                return 42;
            }
            Task<int> Foo4()
            {
                // return Task.Run(() => 42); // 这个方法会凭空创造出一个Task, 实际上是直接返回结果的，Task是引用类型，会浪费托管内存以及将来GC的调用
                return Task.FromResult(42);
            }
        }

        // async void
        // 无法等待
        // 缺少了 Task 包含的异步报错信息
        [Test]
        public async void VoidAsyncCall()
        {
            try
            {
                // await Foo(); // 返回值是 void , 无法等待
                Foo();
            }
            catch (System.Exception ex)
            {
                ex.Log();
            }

            async void Foo()
            {
                await Task.Delay(1000);
                throw new System.Exception("Somethings war wrong!"); // 返回值是 void, 异常无法被外部捕获
            }   
        }
    }
}