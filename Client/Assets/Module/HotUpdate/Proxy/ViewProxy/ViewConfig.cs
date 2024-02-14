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
        private readonly ReadOnlyDictionary<Type, (string path, VIEW_HIERARCY hierarchy)> typeContainer;
        private readonly ReadOnlyDictionary<VIEW, (string path, VIEW_HIERARCY hierarchy)> viewContainer;
        
        [Inject]
        public ViewConfig()
        {
            typeContainer = new ReadOnlyDictionary<Type, (string path, VIEW_HIERARCY hierarchy)>(new Dictionary<Type, (string, VIEW_HIERARCY)>());
            viewContainer = new ReadOnlyDictionary<VIEW, (string path, VIEW_HIERARCY hierarchy)>(new Dictionary<VIEW, (string, VIEW_HIERARCY)>());
            
            Subscribe<HelloScreen>("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame);
            
            Subscribe(VIEW.HelloScreen, "Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame);
        }

        private void Subscribe<T>(string path, VIEW_HIERARCY hierarchy) where T : class, IView
        {
            var type = typeof(T);
            if (!typeContainer.TryAdd(type, (path, hierarchy)))
            {
                $"重复订阅 {nameof(T)}: {type}".FrameError();
            }
        }

        private void Subscribe(VIEW view, string path, VIEW_HIERARCY hierarchy)
        {
            if (!viewContainer.TryAdd(view, (path, hierarchy)))
            {
                $"重复订阅 {nameof(VIEW)}: {view}".FrameError();
            }
        }

        string IViewConfig.ViewLayoutPath()
        {
            return viewLayoutPath;
        }

        (string? path, VIEW_HIERARCY? hierarchy) IViewConfig.Get<T>()
        {
            var type = typeof(T);
            if (!typeContainer.TryGetValue(type, out var result))
            {
                $"未订阅 {nameof(T)}: {type}".FrameError();
                return (null, null);
            }
            return result;
        }

        (string? path, VIEW_HIERARCY? hierarchy) IViewConfig.Get(VIEW view)
        {
            if (!viewContainer.TryGetValue(view, out var result))
            {
                $"未订阅 {nameof(VIEW)}: {view}".FrameError();
                return (null, null);
            }
            return result;
        }
    }
}

