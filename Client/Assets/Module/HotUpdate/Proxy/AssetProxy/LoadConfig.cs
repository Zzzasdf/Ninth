using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.HotUpdate
{
    public class LoadConfig: IJson
    {
        public List<AssetRef> AssetRefList = new();
    }
}