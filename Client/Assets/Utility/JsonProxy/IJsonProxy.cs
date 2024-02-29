using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.Utility
{
    public interface IJsonProxy
    {
        // Generics
        UniTask<TKey> ToObjectAsync<TKey>(CancellationToken cancellationToken = default, bool newIfNotExist = false) where TKey : class, IJson, new();
        TKey ToObject<TKey>(int markBit = 0, bool newIfNotExist = false) where TKey : class, IJson, new();
        UniTask ToJsonAsync<TKey>(CancellationToken cancellationToken = default, bool throwEmptyError = true) where TKey : class, IJson, new();
        void ToJson<TKey>(int markBit = 0, bool throwEmptyError = true) where TKey : class, IJson, new();
        string GetPath<TKey>(int markBit = 0) where TKey : class, IJson, new();
        
        // EnumType
        UniTask<TResult> ToObjectAsync<TResult, TKeyEnum>(CancellationToken cancellationToken = default, bool newIfNotExist = false) where TResult : class, IJson, new() where TKeyEnum : Enum;
        TResult ToObject<TResult, TKeyEnum>(bool newIfNotExist = false) where TResult : class, IJson, new() where TKeyEnum : Enum;
        UniTask ToJsonAsync<T, TKeyEnum>(CancellationToken cancellationToken = default, bool throwEmptyError = true) where T : class, IJson, new() where TKeyEnum : Enum;
        void ToJson<T, TKeyEnum>(bool throwEmptyError = true) where T : class, IJson, new() where TKeyEnum : Enum;
        string GetPathByEnumType<TKeyEnum>() where TKeyEnum : Enum;
        
        // Enum
        UniTask<TResult> ToObjectAsync<TResult>(Enum key, CancellationToken cancellationToken = default, bool newIfNotExist = false) where TResult : class, IJson, new();
        TResult ToObject<TResult>(Enum key, bool newIfNotExist = false) where TResult : class, IJson, new();
        UniTask ToJsonAsync<T>(Enum key, CancellationToken cancellationToken = default, bool throwEmptyError = true) where T : class, IJson, new();
        void ToJson<T>(Enum key, bool throwEmptyError = true) where T : class, IJson, new();
    }
}