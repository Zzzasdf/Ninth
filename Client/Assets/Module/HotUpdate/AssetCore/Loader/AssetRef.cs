using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class AssetRef
    {
        /// <summary>
        /// 这个资源的相对路径
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// 这个资源所属的BundleRef对象
        /// </summary>
        public BundleRef BundleRef;

        /// <summary>
        /// 这个资源所依赖的BundleRef对象列表
        /// </summary>
        public List<BundleRef> Dependencies;

        /// <summary>
        /// 资源状态
        /// </summary>
        public AssetStatus AssetStatus;

        /// <summary>
        /// 加载时自身Asset的引用
        /// </summary>
        public Object Asset;

        /// <summary>
        /// 依赖此资源的对象
        /// </summary>
        public List<GameObject> BeGameObjectDependedList;

        public AssetRef()
        {
            AssetStatus = AssetStatus.Empty;
        }
    }
}