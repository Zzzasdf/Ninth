using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Ninth.HotUpdate;
using System.Linq;
using System.Diagnostics;
using System;

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
    //    => 控制 Task 的调度方式和运行线程
    //      => 线程池线程 Default
    //      => 当前线程 CurrentThread
    //      => 单线程上下文 STAThread
    //      => 长时间运行线程 LongRunning
    //    => 优先级、上下文、执行状态等

    // 7、一发即忘（Fire-and-forget）
    //  => 调用一个异步方法，但是并不使用 await 或阻塞的方式去等待它的结束
    //  => 无法观察任务的状态（是否完成、是否报错等）

    // 8、简单任务
    //  => 如何创建异步任务？
    //    => Task.Run()
    //    => Task.Factory.StartNew()
    //      => 提供更多功能，比如 TaskCreationOptions.LongRunning
    //      => Task.Run 相当于简化版
    //    => new Task + Task.Start()
    //  => 如何同时开启多个异步任务？
    //    => 不要 for 循环中使用 await
    //    => Task.WhenAll()、Task.WhenAny()
    //  => 任务如何取消？
    //    => CancellationTokenSource + CancellationToken
    //      => CTS 实现了 IDisposable 接口，所以需要释放
    //      => CTS 还可以传入一个 TimeSpan，表示超时后自己自动取消；或调用 CancelAfter() 方法
    //    => cts.Cancel() & Token.IsCancellationRequested
    //      => Token.ThrowIfCancellationRequested()
    //    => 推荐异步方法都带上 CancellationToken 这一传参
    //      => "我可以不用，但你不能没有"
    //    => 任务取消时的对策
    //      => 1、抛出异常
    //        => OperationCanceledException & TaskCanceledException
    //        => TaskCanceledException 是 OperationCanceledException 的子类。内置的基本上都是抛出 TaskCanceledException，可以全部都用 OperationCanceledException 来捕获
    //        => Token.ThrowIfCancellationRequested() 抛出的是 OperationCanceledException
    //      => 2、提前返回
    //        => Task.FromCancelled
    //      => 3、记得善后
    //        => try catch finally
    //        => Token.Register()
    //          => 可多次注册
    //          => 后注册的先调用
    //    => 其他
    //      => Task.Run(cancellationToken)
    //      => Cancellation 所在的命名空间
    //      => AsyncRelayCommand
    //  => 任务超时如何实现？
    //    => Task.WhenAny
    //    => (.Net 6.0+) WaitAsync
    //  => 在异步任务重汇报进度？
    //  => 如何在同步方法中调用异步方法？

    // 9、常见误区
    //  => 异步一定会多线程？
    //    => 异步编程不必需要多线程来实现
    //      => 时间片轮转调度
    //    => 比如可以在单个线程上使用异步 I/O 或时间驱动的编程模型（EAP）
    //    => 单线程异步：自己定好计时器，到时间之前先去做别的时间
    //       多线程异步：将任务交给不同的线程，并由自己来进行指挥调度
    //  => 异步方法一定要写成 async Task?
    //    => async 关键字只是用来配合 await 使用，从而将方法包装成状态机
    //    => 本质上仍然是 Task，只不过提供了语法糖，并且函数体重可以直接 return Task 的泛型类型
    //    => 接口中无法声明 async Task
    //  => await 一定会切换同步上下文？
    //    => 在使用 await 关键字调用并等待一个异步任务时，异步方法不一定会立刻来到新的线程上
    //    => 如果 await 了一个已经完成的任务（包括Task.Delay(0))，会直接获得结果
    //  => 异步可以全面取代多线程？
    //    => 异步编程与多线程有一定关系，但两者并不是可以完全互相替代
    //  => Task.Result 一定会阻塞当前线程？
    //    => 如果任务已经完成，那么 Task.Result 可以直接得到结果
    //  => 开启的异步任务一定不会阻塞当前线程？
    //    => await 关键字不一定会立刻释放当前线程，所以如果调用的异步方法中存在阻塞（如 Thread.Sleep(0))，那么依旧会阻塞当前上下文对应的线程
    
    // 10、同步机制
    //  => 传统方法（不适用异步编程，都是阻塞的方法）
    //    => Monitor（lock)
    //    => Mutex
    //    => Semaphore
    //    => EventWaitHandle
    //  => 轻量型
    //    => SemaphoreSlim （原生的几乎只有这个是适合异步编程）
    //    => ManualResetEventSlim
    //  => 并发集合
    //  => 第三方库
    //    => AsyncManualResetEvent (来自Microsoft.VisualStudio.Threading)
    //    => AsyncLock (来自Nito.AsyncEx)

    // eg！另有 ValueTask 值类型版本
    // 无法在 lock 里面使用 await, 因为进入是由线程 A 把它锁上的，而退出时有可能是从线程 B 退出的，这显然是不可能的
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

            async Task FooAsync(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                "Before".Log("Async {0}");
                await Task.Delay(2000, token).ConfigureAwait(false); // 关闭同步上下文，不会回到原线程
                (await GetValueAsync(token)).Log();
                "After".Log("Async {0}");
            }

            async Task<int> GetValueAsync(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                await Task.Delay(2000, token);
                return 42;
            }
        }

        // Task 同步方法写法
        // 同步调用异步：被调用的异步不要让其回到原线程，因为原线程此时被阻塞，异步回到的原线程正在被阻塞，死锁
        // 解决方案：让被调用的异步配置ConfigureAwait(false), 让其等待后的线程继续执行
        // 异步方法若有可能被使用同步方法的方式调用，第一个等待的方法请配置成ConfigureAwait(false)
        [Test]
        public void TaskSync()
        {
            Foo().GetAwaiter().GetResult();

            async Task Foo(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                await Task.Delay(1000, token).ConfigureAwait(false); // eg！！
            }

            async Task Foo1(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                return;
            }
            Task? Foo2(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                return null;
            }

            async Task<int> Foo3Async(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                return 42;
            }
            Task<int> Foo4(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
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

            async void Foo(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                await Task.Delay(1000);
                throw new System.Exception("Somethings war wrong!"); // 返回值是 void, 异常无法被外部捕获
            }   
        }

        // 创建异步任务
        [Test]
        public async void CreatTask()
        {
            "HeavyJob".Log("Before {0}");
            // var res = await Task.Run(HeavyJob);
            var res = await Task.Factory.StartNew(HeavyJob);
            res.Log("Result {0}");
            "HeavyJob".Log("After {0}");

            int HeavyJob()
            {
                "HeavyJob".Log();
                Thread.Sleep(3000);
                return 42;
            }
        }

        // 多个异步任务
        [Test]
        public async void TasksAsync()
        {
            var inputs = Enumerable.Range(1, 10).ToArray();
            var sem = new SemaphoreSlim(3, 3);

            // var outputs = new List<int>();
            // foreach (var input in inputs)
            // {
            //     outputs.Add(await HeavyJob(input));
            // } // 每轮依然要等待
            // outputs.Log();

            var tasks = new List<Task<int>>();
            foreach(var input in inputs)
            {
                tasks.Add(HeavyJob(input));
            }
            await Task.WhenAll(tasks);
            var outputs = tasks.Select(x => x.Result).ToArray();
            outputs.Log();

            async Task<int> HeavyJob(int input, CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                await sem.WaitAsync();
                await Task.Delay(1000, token);
                sem.Release();
                return input * input.Log();
            }
        } 

        // 取消任务
        [Test]
        public async void TaskCancel()
        {
            var cts = new CancellationTokenSource();
            try
            {
                var task = Task.Delay(10000, cts.Token);
                Thread.Sleep(2000);
                cts.Cancel();
                "Before".Log("await {0}");
                await task;
                "After".Log("await {0}");
            }
            catch(TaskCanceledException)
            {
                "Task canceled".Log();
            }
            finally
            {
                cts.Dispose();
            }
        }

        // 取消任务
        [Test]
        public async void TaskCancelCollection()
        {
            // await Example1();
            // await Example2();
            await Example3();

            async Task Example1()
            {
                var cts = new CancellationTokenSource();
                var token = cts.Token;
                var sw = Stopwatch.StartNew();
                try
                {
                    var cancelTask = Task.Run(async () =>
                    {
                        await Task.Delay(3000);
                        cts.Cancel();
                    });
                    await Task.WhenAll(Task.Delay(5000, token), cancelTask);
                }
                catch (TaskCanceledException e)
                {
                    e.Log();
                }
                finally
                {
                    cts.Dispose();
                }
                sw.ElapsedMilliseconds.Log("Task completed in {0} ms");
            }

            async Task Example2()
            {
                // using var cts = new CancellationTokenSource(3000);
                // using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                using (var cts = new CancellationTokenSource())
                {
                    cts.CancelAfter(TimeSpan.FromSeconds(3));
                    var token = cts.Token;
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        await Task.Delay(5000, token);
                    }
                    catch (TaskCanceledException e)
                    {
                        e.Log();
                    }
                    sw.ElapsedMilliseconds.Log("Task completed in {0} ms");
                }
            }

            async Task Example3()
            {
                // using (var cts = new CancellationTokenSource()))
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    var token = cts.Token;
                    var sem = new SemaphoreSlim(3, 3);
                    sem.WaitAsync(token);
 
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        await Task.Delay(5000, token);
                    }
                    catch (TaskCanceledException e)
                    {
                        e.Log();
                    }
                    sw.ElapsedMilliseconds.Log("Task completed in {0} ms");
                }
            }
        }

        // 传入 Token 的方法
        [Test]
        public async void TransferToken()
        {
            using(var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                try
                {
                    await FooAsync(10000, cts.Token);
                    "finish".Log();
                }
                catch(TaskCanceledException)
                {
                    "cancel".Log();
                }
                "go on".Log();
            }

            async Task FooAsync(int millisecondsDelay, CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                "enter delay".Log();
                await Task.Delay(millisecondsDelay, token);
            }
        }

        // 自定义传入 Token 的方法
        [Test]
        public async void TransferTokenByCustomFunc()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    await(FooAsync1(cts.Token));

                    // cts.Cancel();
                    // (await FooAsync2(cts.Token)).Log();

                    "finish".Log();
                }
                catch(OperationCanceledException e) // ThrowIfCancellationRequested 抛出的是这个
                {
                    e.Log();
                }
                "go on".Log();
            }

            Task FooAsync1(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                return Task.Run(async () =>
                {
                    // if(token.IsCancellationRequested)
                    //     token.ThrowIfCancellationRequested();
                    while(true)
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();
                        await Task.Delay(1000);
                        "Pooling...".Log();
                    }
                }, token); // 等价与上面注释的代码
            }

            Task<string> FooAsync2(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                var task = new Task(() => {});
                if(token.IsCancellationRequested)
                    return Task.FromCanceled<string>(token);
                return Task.FromResult("done");
            }
        }

        // 使用 Regiter 做取消的善后工作
        // 可多次注册，后注册先调用
        [Test]
        public async void RegisterRehabilitate()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                cts.Token.Register(() => "Cancelation requested in Main Thread".Log());
                try
                {
                    cts.Token.Register(() => "Cancelation requested in Try Block".Log());
                    await FooAsync(cts.Token);
                }
                catch(TaskCanceledException e)
                {
                    e.Log();
                }
            }
            
            async Task FooAsync(CancellationToken? cancellationToken = null)
            {
                var token = cancellationToken ?? CancellationToken.None;
                await Task.Delay(10000, token);
            }
        }

        // 异步超时机制
        // eg!! 超时不取消，原任务会继续进行
        [Test]
        public async void TaskTimeout()
        {
            using (var cts = new CancellationTokenSource())
            {
                var fooTask = FooAsync(cts.Token);
                var completedTask = await Task.WhenAny(fooTask, Task.Delay(TimeSpan.FromSeconds(2)));
                if(completedTask != fooTask)
                {
                    cts.Cancel(); // eg!!
                    await fooTask;
                    "Timeout...".Log();
                }
                "Done.".Log();
            }

            async Task FooAsync(CancellationToken? cancellationToken = null)
            {
                try
                {
                    "Foo start...".Log();
                    await Task.Delay(5000);
                    "Foo end...".Log();
                }
                catch (OperationCanceledException)
                {
                    "Foo cancelled...".Log();
                }
            }
        }
    }
}