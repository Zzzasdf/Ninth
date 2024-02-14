using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class JsonConfig: IJsonConfig
    {
        private readonly ReadOnlyDictionary<Enum, string?> jsonContainer;
            
        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            jsonContainer = new ReadOnlyDictionary<Enum, string?>(new Dictionary<Enum, string?>());
            
            Subscribe(VERSION_PATH.StreamingAssets, pathProxy.Get(VERSION_PATH.StreamingAssets));
            Subscribe(VERSION_PATH.PersistentData, pathProxy.Get(VERSION_PATH.PersistentData));
            Subscribe(VERSION_PATH.PersistentDataTemp, pathProxy.Get(VERSION_PATH.PersistentDataTemp));
        }
        
        private void Subscribe(Enum e, string? path)
        {
            if (!jsonContainer.TryAdd(e, path))
            {
                $"重复订阅 {e.GetType().Name}: {e}".FrameError();
            }
        }

        string? IJsonConfig.Get(Enum e)
        {
            if (!jsonContainer.TryGetValue(e, out var result))
            {
                $"未订阅 {e.GetType().Name}: {e}".FrameError();;
                return null;
            }
            return result;
        }
    }
}
