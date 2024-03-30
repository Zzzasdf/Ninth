using System;
using System.Collections.Generic;
using Ninth.HotUpdate;
using UnityEngine;

namespace Ninth.Editor
{
    [Serializable]
    public class AssetReference<TAsset, TConfig>
    {
        public TAsset Asset;
        public TConfig Config;
    }
}