using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Ninth.Utility
{
    public interface IJsonProxy
    {
        // Generics
        UniTask<T?> ToObjectAsync<T>(CancellationToken cancellationToken = default, bool unExistLog = true) where T: class, IJson;
        T? ToObject<T>(bool unExistLog = true) where T: class, IJson;
        UniTask ToJsonAsync<T>(T obj, CancellationToken cancellationToken) where T: class, IJson;
        void ToJson<T>(T obj) where T: class, IJson;

        // EnumType
        UniTask<T?> ToObjectAsync<T, TEnum>(CancellationToken cancellationToken = default, bool unExistLog = true) where T : class, IJson where TEnum : Enum;
        T? ToObject<T, TEnum>(bool unExistLog = true) where T : class, IJson where TEnum : Enum;
        UniTask ToJsonAsync<T, TEnum>(T obj, CancellationToken cancellationToken = default) where T : class, IJson where TEnum : Enum;
        void ToJson<T, TEnum>(T obj) where T : class, IJson where TEnum : Enum;
        
        // Enum
        UniTask<T?> ToObjectAsync<T>(Enum e, CancellationToken cancellationToken = default, bool unExistLog = true) where T : class, IJson;
        T? ToObject<T>(Enum e, bool unExistLog = true) where T : class, IJson;
        UniTask ToJsonAsync<T>(T obj, Enum e, CancellationToken cancellationToken = default) where T: class, IJson;
        void ToJson<T>(T obj, Enum e) where T : class, IJson;
    }
}