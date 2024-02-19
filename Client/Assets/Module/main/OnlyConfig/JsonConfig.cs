using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            baseSubscribe = new BaseSubscribe<Enum, string?>
            {
                [VERSION_PATH.StreamingAssets] = pathProxy.Get(VERSION_PATH.StreamingAssets),
                [VERSION_PATH.PersistentData] = pathProxy.Get(VERSION_PATH.PersistentData),
                [CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData] = pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData),
                [CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData] = pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData),
            };
        }
    }
}
