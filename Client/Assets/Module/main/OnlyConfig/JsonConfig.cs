using System;
using Ninth.Utility;
using VContainer;

namespace Ninth
{
    public class JsonConfig : IJsonConfig
    {
        private readonly SubscribeCollect<string> stringSubscribe;
        SubscribeCollect<string> IJsonConfig.StringSubscribe => stringSubscribe;

        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            {
                var build = stringSubscribe = new SubscribeCollect<string>();
                build.Subscribe(VERSION_PATH.StreamingAssets, pathProxy.Get(VERSION_PATH.StreamingAssets));
                build.Subscribe(VERSION_PATH.PersistentData, pathProxy.Get(VERSION_PATH.PersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData));
            }
        }

    }
}