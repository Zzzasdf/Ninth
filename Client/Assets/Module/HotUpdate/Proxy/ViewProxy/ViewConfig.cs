using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewConfig: IViewConfig
    {
        private readonly EnumTypeSubscribe<string> enumTypeSubscribe;
        private readonly GenericsSubscribe<IView, (string path, VIEW_HIERARCY hierarcy)> genericsSubscribe;
        private readonly CommonSubscribe<VIEW, (string path, VIEW_HIERARCY hierarchy)> commonSubscribe;

        EnumTypeSubscribe<string> IViewConfig.EnumTypeSubscribe => enumTypeSubscribe;
        GenericsSubscribe<IView, (string path, VIEW_HIERARCY hierarcy)> IViewConfig.GenericsSubscribe => genericsSubscribe;
        CommonSubscribe<VIEW, (string path, VIEW_HIERARCY hierarchy)> IViewConfig.CommonSubscribe => commonSubscribe;
        
        [Inject]
        public ViewConfig()
        {
            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<string>();
                build.Subscribe<VIEW_HIERARCY>("Assets/GAssets/RemoteGroup/View/ViewLayout.prefab");
            }
            
            {
                var build = genericsSubscribe = new GenericsSubscribe<IView, (string path, VIEW_HIERARCY hierarcy)>();
                build.Subscribe<HelloScreen>(("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame));
            }

            {
                var build = commonSubscribe = new CommonSubscribe<VIEW, (string path, VIEW_HIERARCY hierarchy)>();
                build.Subscribe(VIEW.HelloScreen, ("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame));
            }
        }
    }
}

