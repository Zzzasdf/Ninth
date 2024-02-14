using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public interface IJsonProxy
    {
        UniTask<T?> ToObjectAsync<T>(Enum e, CancellationToken cancellationToken = default) where T : class;
        UniTaskVoid ToJsonAsync<T>(T obj, Enum e, CancellationToken cancellationToken = default) where T: class; 

    }
}