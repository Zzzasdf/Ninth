using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth.Utility
{
    public interface IJsonProxy
    {
        UniTask<T?> ToObjectAsync<T>(CancellationToken cancellationToken = default) where T: class, IJson;
        T? ToObject<T>() where T: class, IJson;
        UniTaskVoid ToJsonAsync<T>(T obj, CancellationToken cancellationToken) where T: class, IJson;
        void ToJson<T>(T obj) where T: class, IJson;

        UniTask<T?> ToObjectAsync<T, TEnum>(CancellationToken cancellationToken = default) where T : class, IJson where TEnum : Enum;
        T? ToObject<T, TEnum>(CancellationToken cancellationToken = default) where T : class, IJson where TEnum : Enum;
        UniTaskVoid ToJsonAsync<T, TEnum>(T obj, CancellationToken cancellationToken = default) where T : class, IJson where TEnum : Enum;
        void ToJson<T, TEnum>(T obj) where T : class, IJson;
        
        UniTask<T?> ToObjectAsync<T>(Enum e, CancellationToken cancellationToken = default) where T : class, IJson;
        T? ToObject<T>(Enum e) where T : class, IJson;
        UniTaskVoid ToJsonAsync<T>(T obj, Enum e, CancellationToken cancellationToken = default) where T: class, IJson;
        void ToJson<T>(T obj, Enum e) where T : class, IJson;
    }
}