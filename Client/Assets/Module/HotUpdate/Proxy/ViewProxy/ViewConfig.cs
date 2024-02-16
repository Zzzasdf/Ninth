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
        private readonly TypeSubscribe<IView, (string? path, VIEW_HIERARCY? hierarcy)?> typeContainer;
        private readonly BaseSubscribe<VIEW, (string? path, VIEW_HIERARCY? hierarchy)?> viewContainer;
        
        [Inject]
        public ViewConfig()
        {
            this.typeContainer = new TypeSubscribe<IView, (string? path, VIEW_HIERARCY? hierarcy)?>()
                .Subscribe<HelloScreen>(("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame));

            this.viewContainer = new BaseSubscribe<VIEW, (string? path, VIEW_HIERARCY? hierarchy)?>
            {
                [VIEW.HelloScreen] = ("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame),
            };
        }

        string IViewConfig.ViewLayoutPath()
        {
            return viewLayoutPath;
        }

        (string? path, VIEW_HIERARCY? hierarchy)? IViewConfig.Get<T>()
        {
            return typeContainer.Get<T>();
        }

        (string? path, VIEW_HIERARCY? hierarchy)? IViewConfig.Get(VIEW view)
        {
            return viewContainer.Get(view);
        }
    }
}

