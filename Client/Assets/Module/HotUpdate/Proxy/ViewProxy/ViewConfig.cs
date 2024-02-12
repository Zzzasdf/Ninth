using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewConfig: IViewConfig
    {
        private readonly string viewLayoutPath = "Assets/GAssets/RemoteGroup/View/ViewLayout.prefab";
        private readonly ReadOnlyDictionary<Type, (string path, ViewHierarchy hierarchy)> mapContainer;
        
        [Inject]
        public ViewConfig()
        {
            var tempContainer = new Dictionary<Type, (string, ViewHierarchy)>();
            mapContainer = new ReadOnlyDictionary<Type, (string path, ViewHierarchy hierarchy)>(tempContainer);

            Subscribe<HelloScreen>("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", ViewHierarchy.Frame);
            return;

            void Subscribe<T>(string path, ViewHierarchy hierarchy) where T: IView
            {
                var type = typeof(T);
                if (!tempContainer.TryAdd(type, (path, hierarchy)))
                {
                    type.FrameError("重复注册 View: {0}");
                }
            }
        }

        string IViewConfig.ViewLayoutPath() => viewLayoutPath;

        ReadOnlyDictionary<Type, (string path, ViewHierarchy hierarchy)> IViewConfig.MapContainer() => mapContainer;
    }
}

