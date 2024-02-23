using System;
using Ninth.Utility;
using VContainer;

namespace Ninth
{
    public class JsonConfig : IJsonConfig
    {
        private readonly GenericsSubscribe<IJson, string> genericsSubscribe;
        private readonly EnumTypeSubscribe<string> enumTypeSubscribe;
        private readonly CommonSubscribe<Enum, string> commonSubscribe;

        GenericsSubscribe<IJson, string> IJsonConfig.GenericsSubscribe => genericsSubscribe;
        EnumTypeSubscribe<string> IJsonConfig.EnumTypeSubscribe => enumTypeSubscribe;
        CommonSubscribe<Enum, string> IJsonConfig.CommonSubscribe => commonSubscribe;

        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            {
                var build = genericsSubscribe = new GenericsSubscribe<IJson, string>();
            }

            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<string>();
            }

            {
                var build = commonSubscribe = new CommonSubscribe<Enum, string>();
                build.Subscribe(VERSION_PATH.StreamingAssets, pathProxy.Get(VERSION_PATH.StreamingAssets));
                build.Subscribe(VERSION_PATH.PersistentData, pathProxy.Get(VERSION_PATH.PersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData));
            }
        }
    }
}