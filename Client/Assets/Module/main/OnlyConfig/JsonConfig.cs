using System;
using Ninth.Utility;
using VContainer;

namespace Ninth
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig(IPathProxy pathProxy)
        {
            genericsSubscribe = new GenericsSubscribe<IJson, string>();
            
            enumTypeSubscribe = new EnumTypeSubscribe<string>();
            
            commonSubscribe = new CommonSubscribe<Enum, string>
            {
                [VERSION_PATH.StreamingAssets] = pathProxy.Get(VERSION_PATH.StreamingAssets),
                [VERSION_PATH.PersistentData] = pathProxy.Get(VERSION_PATH.PersistentData),
                [CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData] = pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData),
                [CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData] = pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData),
            };
        }
    }
}
