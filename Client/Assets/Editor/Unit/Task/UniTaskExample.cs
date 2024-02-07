using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;
using Ninth.HotUpdate;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Ninth.Editor
{
    // https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
    // 轻量型异步插件UniTask

    // 1、原生的异步方案存在问题
    //  => Update / LateUpdate 等
    //    => 逻辑复杂，容易出错
    //    => Update 中代码太多，牵一发动全身
    //    => 会引入很多成员变量，增加代码复杂度
    //    => 必须依托于 monobehaviour
    //  => 协程
    //    => 消耗大
    //    => 无法进行异常处理
    //    => 必须依托于 monobehaviour
    
    // 2、替代方案 async / await Task
    //  => 优点
    //    => 解决了 Update 的复杂度问题
    //    => 解决了对 monobehaviour 的依赖
    //    => 可以进行 try catch
    //  => 缺点
    //    => Task 消耗也不小，涉及到线程调度，相当重
    //    => Task 是跨线程，不符合 Unity 引擎设计
    
    // 3、替代方案 async / await UniTask
    //  => 优点
    //    => 继承了 async / await Task 所有优点
    //  => 进一步改进
    //    => 使用值类型实现 UniTask，插件本身无 GC
    
    // 4、基础使用
    //  => Unity 的异步操作直接进行
    //    => 异步读取（文件，场景
    //    => 网络请求
    //  => 延时操作
    //    => Yield (下次调用)  最快方式
    //    => NextFrame  等于 yield return null
    //    => WaitForEndOfFrame
    //  => 等待时机
    //    => WaitUntil
    //    => WaitUntilValueChanged
    //  => 协程相关
    //    => 协程转换为 UniTask
    //    => UniTask 转换为协程
    //    => 等待 IEnumrator
    //  => Task 相关
    //    => 切换到其他线程
    //    => 切换会主线程
    //  => 回调转换为 UniTask
    
    // 5、进阶使用
    //  => 取消 Cancellation
    //    => 创建新的 CancellationToken 传入
    //    => 依托于 gameObject, GetCancellationTokenOnDestroy
    //  => 异常处理
    //    => 直接 try / catch
    //    => SuppressCancellationThrow  返回是否已经遣返
    //  => 超时处理
    //    => token 调用 CancelAfterSlim
    //    => try catch, 并传入 token, 超时会自动报异常
    //    => 使用TimeoutController
    //  => 事件处理
    //    => lambda 表达式
    //      => UniTask.Action(async ()=> { await UniTask.Yield();});
    //      => UniTask.UnityAction(async ()=> { await UniTask.Yield();});
    //    => ugui 事件转化为 UniTask     ... AsAsyncEnumerable
    //    => monobehaviour 消息事件转化   AsyncTriggers
    //  => C# 8.0 特性
    //    => await foreach
    //    => 异步迭代器
    //  => 异步 Linq
    //    => UniTaskAsyncEnumerable
    //  => Forgot()
    //  => 响应式组件
    //    => AsyncReactiveProperty<T>
    //  => 额外支持
    //    => Dotween
    //      => await xx.DoMove()
    //      => WhenAll
    //    => addressable asset system 所有相关异步操作
    //    => TextMeshPro
    //  => 注意事项  每个 UniTask 只能 await 一次
    
    public class UniTaskExample: UnityEditor.Editor
    {   
        // UniTaskTest.cs
    }
}
