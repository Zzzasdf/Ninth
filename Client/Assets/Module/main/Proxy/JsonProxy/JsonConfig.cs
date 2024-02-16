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
        private readonly BaseSubscribe<Enum, string?> jsonSubscribe;
        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            jsonSubscribe = new BaseSubscribe<Enum, string?>
            {
                [VERSION_PATH.StreamingAssets] = pathProxy.Get(VERSION_PATH.StreamingAssets),
                [VERSION_PATH.PersistentData] = pathProxy.Get(VERSION_PATH.PersistentData),
                [VERSION_PATH.PersistentDataTemp] = pathProxy.Get(VERSION_PATH.PersistentDataTemp),
            };
        }

        public string? Get(Enum e)
        {
            return jsonSubscribe.Get(e);
        }
    }
}
