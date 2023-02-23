using System.Collections;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public class LocalLoadConfig: LoadConfig, IJson
    {

    }

    public class RemoteLoadConfig: LoadConfig, IJson
    {

    }

    public class DllLoadConfig: LoadConfig, IJson
    {

    }

    public class LoadConfig
    {
        public List<AssetRef> AssetRefList;
    }
}