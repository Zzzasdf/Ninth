using System;
using VContainer;

namespace Ninth.Utility
{
    public class JsonConfig : IJsonConfig
    {
        private readonly TypeSubscriber<string> typeSubscriber;
        private readonly Subscriber<Enum, string> subscriber;
        TypeSubscriber<string> IJsonConfig.TypeSubscriber => typeSubscriber;
        Subscriber<Enum, string> IJsonConfig.Subscriber => subscriber;
        

        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            {
                var build = subscriber = new Subscriber<Enum, string>();
                build.Subscribe(VERSION_PATH.PersistentDataTemp, pathProxy.Get(VERSION_PATH.PersistentDataTemp));
                build.Subscribe(VERSION_PATH.PersistentData, pathProxy.Get(VERSION_PATH.PersistentData));
                build.Subscribe(VERSION_PATH.StreamingAssets, pathProxy.Get(VERSION_PATH.StreamingAssets));

                build.Subscribe(CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData));

                build.Subscribe(CONFIG_PATH.LoadConfigPathByLocalGroupByStreamingAssets, pathProxy.Get(CONFIG_PATH.LoadConfigPathByLocalGroupByStreamingAssets));
                build.Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByStreamingAssets, pathProxy.Get(CONFIG_PATH.LoadConfigPathByRemoteGroupByStreamingAssets));
                build.Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData, pathProxy.Get(CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData));
            }
        }
    }
}