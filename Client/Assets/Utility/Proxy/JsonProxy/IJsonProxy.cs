using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.Utility
{
    public interface IJsonProxy
    {
        // Generics
        UniTask<TKey> ToObjectAsync<TKey>(CancellationToken cancellationToken = default, int markBit = 0, Func<TKey>? notExistHandle = null) where TKey : class, IJson;
        TKey ToObject<TKey>(int markBit = 0, Func<TKey>? notExistHandle = null) where TKey : class, IJson;
        UniTask ToJsonAsync<TKey>(CancellationToken cancellationToken = default, int markBit = 0) where TKey : class, IJson;
        void ToJson<TKey>(int markBit = 0) where TKey : class, IJson;
        bool CacheExists<TKey>(int markBit = 0) where TKey : class, IJson;

        // EnumType
        UniTask<TResult> ToObjectAsync<TResult, TKeyEnum>(CancellationToken cancellationToken = default, int markBit = 0, Func<TResult>? notExistHandle = null) where TResult : class, IJson where TKeyEnum : Enum;
        TResult ToObject<TResult, TKeyEnum>(int markBit = 0, Func<TResult>? notExistHandle = null) where TResult : class, IJson where TKeyEnum : Enum;
        UniTask ToJsonAsync<T, TKeyEnum>(CancellationToken cancellationToken = default, int markBit = 0) where T : class, IJson where TKeyEnum : Enum;
        void ToJson<T, TKeyEnum>(int markBit = 0) where T : class, IJson where TKeyEnum : Enum;
        bool CacheExists<T, TKeyEnum>(int markBit = 0) where T : class, IJson where TKeyEnum : Enum;
        
        // Enum
        UniTask<TResult> ToObjectAsync<TResult>(Enum key, CancellationToken cancellationToken = default, int markBit = 0, Func<TResult>? notExistHandle = null) where TResult : class, IJson;
        TResult ToObject<TResult>(Enum key, int markBit = 0, Func<TResult>? notExistHandle = null) where TResult : class, IJson;
        UniTask ToJsonAsync<T>(Enum key, CancellationToken cancellationToken = default, int markBit = 0) where T : class, IJson;
        void ToJson<T>(Enum key, int markBit = 0) where T : class, IJson;
        bool CacheExists<T>(Enum key, int markBit = 0) where T : class, IJson;
        
        // Base
        UniTask<T> ToObjectAsync<T>(string path, CancellationToken cancellationToken, Func<T>? notExistHandle = null) where T : class, IJson;
        T ToObject<T>(string path, Func<T>? notExistHandle = null) where T : class, IJson;
        UniTask ToJsonAsync<T>(T obj, string path, CancellationToken cancellationToken) where T : class, IJson;
        void ToJson<T>(T obj, string path) where T : class, IJson;
        bool CacheExists<T>(string path) where T : class, IJson;
    }
}