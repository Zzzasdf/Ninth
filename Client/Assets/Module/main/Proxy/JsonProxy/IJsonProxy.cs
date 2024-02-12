using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public interface IJsonProxy
    {
        UniTask<T?> ToObject<T>(JsonFile jsonFile, CancellationToken cancellationToken = default);
        UniTaskVoid ToJson<T>(T obj, JsonFile jsonFile, CancellationToken cancellationToken = default);
    }
}