using System.Collections;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public class LocalLoadConfig: LoadConfig
    {

    }

    public class RemoteLoadConfig: LoadConfig
    {

    }

    public class DllLoadConfig: LoadConfig
    {

    }

    public class LoadConfig: IModel
    {
        public List<AssetRef> AssetRefList;
    }
}