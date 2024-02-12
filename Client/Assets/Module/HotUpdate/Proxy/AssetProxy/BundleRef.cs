using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class BundleRef
    {
        /// <summary>
        /// 资源包的名字
        /// </summary>
        public string? BundleName;

        /// <summary>
        /// 资源包定位
        /// </summary>
        public AssetLocate AssetLocate;

        /// <summary>
        /// 加载时自身Bundle的引用
        /// </summary>
        public AssetBundle? Bundle;

        /// <summary>
        /// 加载时所有依赖此bundle的Asset集合
        /// </summary>
        public List<AssetRef>? BeAssetRefDependedList;
    }
}