using System.Collections;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public class LocalLoadConfig: LoadConfig, IJsonProxy
    {

    }

    public class RemoteLoadConfig: LoadConfig, IJsonProxy
    {

    }

    public class DllLoadConfig: LoadConfig, IJsonProxy
    {

    }

    public class LoadConfig
    {
        public List<AssetRef> AssetRefList;
    }
}