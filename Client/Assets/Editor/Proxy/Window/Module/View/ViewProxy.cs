using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class ViewProxy: BaseAssetModuleProxy<BaseView, BaseChildView>
    {
        [Inject]
        public ViewProxy(ViewConfig viewConfig)
        {
            baseAssetModuleConfig = viewConfig;
        }
    }
}
