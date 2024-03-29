using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ninth.HotUpdate
{
    [CreateAssetMenu(fileName = "ViewConfigSO", menuName = "Config/ViewConfigSO")]
    public class ViewConfig : ScriptableObject, IViewConfig
    {
        public VIEW_HIERARCHY DefaultHierarchy;
        public int DefaultWeight;
        public int DefaultChildWeight;
        
        public ViewLayout ViewLayout;
        public string ViewLayoutPath;
        
        public List<ViewInfo> ViewInfos = new();

        private Subscriber<string, string> layoutSubscriber;
        private Subscriber<string, ViewInfo> viewInfoSubscriber;
        
        public Subscriber<string, string> LayoutSubscriber
        {
            get
            {
                if (layoutSubscriber == null)
                {
                    layoutSubscriber = new Subscriber<string, string>();
                    layoutSubscriber.Subscribe(typeof(ViewLayout).Name, ViewLayoutPath);
                }
                return layoutSubscriber;
            }
        }
        public Subscriber<string, ViewInfo> ViewInfoSubscriber
        {
            get
            {
                if (viewInfoSubscriber == null)
                {
                    viewInfoSubscriber = new Subscriber<string, ViewInfo>();
                    // for (var i = 0; i < Keys.Count; i++)
                    // {
                    //     viewInfoSubscriber.Subscribe(Keys[i].GetType().Name, new ViewInfo(Paths[i], Hierarchys[i], Weights[i]));
                    // }
                }
                return viewInfoSubscriber;
            }
        }
        
        [Serializable]
        public class ViewInfo
        {
            public string Key;
            public string Path;
            public VIEW_HIERARCHY Hierarchy;
            public int Weight;
            public List<ChildViewInfo>? ChildViewInfos;

            public void Deconstruct(out string path, out VIEW_HIERARCHY hierarchy, out int weight)
            {
                path = this.Path;
                hierarchy = this.Hierarchy;
                weight = this.Weight;
            }
        }

        [Serializable]
        public class ChildViewInfo
        {
            public string Key;
            public string Path;
            public int Weight;
        }
    }
}