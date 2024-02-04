using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ninth.HotUpdate;
using NUnit.Framework;

namespace Ninth.Editor
{
    // 1、经常阻塞
    // 2、适用场景
    //  => 适合 CPU 密集型操作 ( UI 线程、轮询操作)
    //  => 适合长期运行的任务
    //  => 线程的创建与销毁开销较大
    //  => 提供更底层的控制，操作线程、锁、信号量等
    //  => 线程不易于传参及返回
    //  => 线程的代码书写较为繁琐
    public class ThreadExample 
    {
        // 线程安全
        [Test]
        public void ThreadSafety()
        {
            Example1();
            // Example2();
            

            void Example1()
            {
                const int total = 10;
                int count = 0;
                // object lockObj = new Object();

                var thread1 = new Thread(ThreadMethod);
                var thread2 = new Thread(ThreadMethod);

                thread1.Start();
                thread2.Start();

                thread1.Join();
                thread2.Join();

                $"Count: {count}".Log();

                
                void ThreadMethod()
                {
                    for(int i = 0; i < total; i++)
                        // lock(lockObj)
                            // count++;
                            Interlocked.Increment(ref count); // 原子操作，自增
                }
            }

            void Example2()
            {
                var queue = new Queue<int>();
                var lockObj = new object();

                var producer = new Thread(AddNumbers);
                var consumer1 = new Thread(ReadNumbers);
                var consumer2 = new Thread(ReadNumbers);

                producer.Start();
                consumer1.Start();
                consumer2.Start();

                producer.Join();
                consumer1.Interrupt();
                consumer2.Interrupt();
                consumer1.Join();
                consumer2.Join();

                void AddNumbers()
                {
                    for(int i = 0; i < 20; i++)
                    {
                        Thread.Sleep(20);
                        queue.Enqueue(i);
                    }
                }

                void ReadNumbers()
                {
                    try
                    {
                        while(true)
                        {
                            lock(lockObj)
                            {
                                if(queue.TryDequeue(out var res))
                                    res.Log();
                            }
                            Thread.Sleep(1);
                        }
                    }
                    catch(ThreadInterruptedException)
                    {
                        "Thread interrupted.".Log();
                    }
                }
            }
        }

        // 自带方法 Parallel、PLINQ
        [Test]
        public void BuiltInMethod()
        {
            var inputs = Enumerable.Range(1, 20).ToArray();
            var outputs = new int[inputs.Length];
            var sw = Stopwatch.StartNew();

            // for (int i = 0; i < inputs.Length; i++)
            // {
            //     output[i] = HeavyJob(inputs[i]);
            // }
            
            // Parallel.For(0, inputs.Length, i => outputs[i] = HeavyJob(inputs[i]));

            outputs = inputs.AsParallel().AsOrdered().Select(x => HeavyJob(x)).ToArray();

            $"Elapsed time: {sw.ElapsedMilliseconds}ms".Log();

            outputs.Log();

            int HeavyJob(int input)
            {
                Thread.Sleep(100);
                return (input * input).Log();
            }
        }
        
        // 线程创建
        [Test]
        public void ThreadCreation()
        {
            var th1 = new Thread(ThreadMethod1);
            th1.Start();
            
            var th2 = new Thread(ThreadMenthod2);
            th2.Start(123);

            void ThreadMethod1() { }
            void ThreadMenthod2(object? obj) { }
        }

        // 线程终止
        [Test]
        public void ThreadTermination()
        {
            // ThreadJoin();
            ThreadInterrupt();

            // 等待线程结束
            void ThreadJoin()
            {
                var th = new Thread((object? obj) =>
                {
                    obj.Log();
                    for(int i = 0; i < 20; i++)
                    {
                        Thread.Sleep(200);
                        "Thread is still running...".Log();
                    }
                    "Thread is finished!".Log();
                }) { IsBackground = true, Priority = ThreadPriority.Normal };
                th.Start(123);
                "In main thread, waiting for thread to finish...".Log();
                th.Join();
                "Done.".Log();
            }

            // 中断线程执行
            // 如果线程中包含一个 while(trye) 循环(常见的后台线程)，那么需要保证包含等待方法（占用率满了，没有其他资源空闲出来抛出异常），如IO操作，Thread.Sleep等
            // Q: 不能使用Abort?
            // 1、使用 Abort 方法来强制终止线程可能导致一些严重的问题，包括资源泄露和不可预测的行为
            // 2、较新版本的 .Net 中如果使用这个方法，会报 PlatformNotSupportedException
            // 3、推荐使用 Thread.Interrupt 或 CancellationToken
            void ThreadInterrupt()
            {
                var th = new Thread((object? obj) =>
                {
                    try
                    {
                        while(true)
                        {
                            Thread.Sleep(0);
                        }
                        // obj.Log();
                        // for (int i = 0; i < 20; i++)
                        // {
                        //     Thread.Sleep(200);
                        //     "Thread is still running...".Log();
                        // }
                    }
                    catch(ThreadInterruptedException)
                    {
                        
                    }
                    finally
                    {
                        "Thread is finished!".Log();
                    }
                   
                }) { IsBackground = true, Priority = ThreadPriority.Normal };
                th.Start(123);
                "In main thread, waiting for thread to finish...".Log();
                Thread.Sleep(1000);
                th.Interrupt();
                "Done.".Log();
            }
        }

        // 锁与信号量
        // 轻量型，且支持了异步版本
        // 1、SemaphoreSlim
        // 2、ManualResetEventSlim
        // 3、ReaderWriterLockSlim
        [Test]
        public void LocksAndSemaphores()
        {
            Semaphore();
            
            // lock
            // 底层是Monitor
            void Lock() { }

            // 互斥锁
            // 可以进程间共享
            void Mutex() { }

            // Semaphore
            // 限制同时使用的线程数量上限
            // 可以进程间共享
            void Semaphore()
            {
                var inputs = Enumerable.Range(1, 20).ToArray();
                var sw = Stopwatch.StartNew();
                var semaphore = new Semaphore(3, 3); // 初始空闲3个线程，最多3个线程同时使用，并非三个固定的线程
                var outputs = inputs.AsParallel().AsOrdered().Select(HeavyJob);

                "Outputs:".Log();
                string.Join(",", outputs).Log();
                $"Elapsed time: {sw.ElapsedMilliseconds}ms".Log();

                semaphore.Dispose(); // !!

                int HeavyJob(int input)
                {
                    semaphore.WaitOne(); // 阻塞，等待有空闲的线程
                    Thread.Sleep(300);
                    semaphore.Release(); // 回收
                    return (input * input).Log();
                }
            }

            // WaitHandle
            // 典型的信号量
            // 1、ManualResetEvent
            // 2、AutoResetEvent
            // Q：Manual与Auto的区别主要在于：
            // 1、如果有多个线程都在用WaitOne等待信号量，那么每次Set(), auto只会释放一个WaitOne，而manual会全部释放
            // 2、调用WaitOne后，auto会自动调用Reset()方法，而Manual则会保持开放
            void WaitHandle() { }

            // 读写锁
            // 允许多个Reader去读
            // 同时只允许一个Writer去写
            // Reader 和 Writer 互斥
            void ReaderWriterLock() { }
        }


        // 不要自己造轮子！
        // 不要自己写锁
        // 使用 .net 提供的方法
        [Test]
        public void Attention()
        {
            // Collections();
            BlockingCollection();

            // 线程安全的单例
            void Lazy() { }

            // 线程安全的集合类型
            // ConcurrentBag、ConcurrentStack、ConcurrentQueue、ConcurrentDictionary
            void Collections() 
            {
                ConcurrentQueue();

                void ConcurrentQueue()
                {
                    var queue = new ConcurrentQueue<int>();

                    var producer = new Thread(AddNumbers);
                    var consumer1 = new Thread(ReadNumbers);
                    var consumer2 = new Thread(ReadNumbers);

                    producer.Start();
                    consumer1.Start();
                    consumer2.Start();

                    producer.Join();
                    consumer1.Interrupt();
                    consumer2.Interrupt();
                    consumer1.Join();
                    consumer2.Join();

                    void AddNumbers()
                    {
                        for(int i = 0; i < 20; i++)
                        {
                            Thread.Sleep(20);
                            queue.Enqueue(i);
                        }
                    }

                    void ReadNumbers()
                    {
                        try
                        {
                            while(true)
                            {
                                if (queue.TryDequeue(out var res))
                                    res.Log();
                                Thread.Sleep(1);
                            }
                        }
                        catch(ThreadInterruptedException)
                        {
                            "Thread interrupted.".Log();
                        }
                    }
                }
            }

            // 阻塞集合
            // 不适用于异步编程
            // 低延迟
            void BlockingCollection() 
            {
                var queue = new BlockingCollection<Message>(new ConcurrentQueue<Message>());

                var sender = new Thread(SendMessageThread);
                var receiver = new Thread(ReceiveMessageThread);

                sender.Start(1);
                receiver.Start(2);

                sender.Join();
                Thread.Sleep(100);
                receiver.Interrupt();
                receiver.Join();

                "Done.".Log();

                void SendMessageThread(object? arg)
                {
                    int id = (int)arg!;

                    for (int i = 1; i <= 20; i++)
                    {
                        queue.Add(new Message(id, i.ToString()));
                        $"Thread {id} send {i}".Log();
                        Thread.Sleep(100);
                    }
                }

                void ReceiveMessageThread(object? id)
                {
                    try
                    {
                        while(true)
                        {
                            var message = queue.Take();
                            $"Thread {id} received {message.Content} from {message.FromId}".Log();
                            Thread.Sleep(1);
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                        $"Thread {id} interrupted".Log();
                    }
                }
            }

            // 通道
            // 高吞吐
            void Channel() { }

            // 原子操作
            void Interlocked() { }

            // 周期任务
            void PeriodicTimer() { }
        }

        public class Message
        {
            public int FromId;
            public string Content;
            public Message(int FromId, string Content)
            {
                this.FromId = FromId;
                this.Content = Content;
            }
        }

        // 线程超时打断机制
        [Test]
        public void ThreadTimeout()
        {
            var thread = new Thread(Foo);
            thread.Start();
            if (!thread.Join(TimeSpan.FromSeconds(10)))
            {
                thread.Interrupt();
            }
            else
            {
                "finish".Log();
            }
            "Done".Log();


            void Foo()
            {
                try
                {
                    "Foo start...".Log();
                    Thread.Sleep(5000);
                    "Foo end...".Log();
                }
                catch(ThreadInterruptedException)
                {
                    "Foo interrupted...".Log();
                }
            }
        }
    }
}
