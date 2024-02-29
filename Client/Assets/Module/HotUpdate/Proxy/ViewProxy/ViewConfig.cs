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
        private readonly SubscribeCollect<string> stringSubscribe;
        private readonly SubscribeCollect<(string path, VIEW_HIERARCY hierarcy)> tupleSubscribe;
        SubscribeCollect<string> IViewConfig.StringSubscribe => stringSubscribe;
        SubscribeCollect<(string path, VIEW_HIERARCY hierarcy)> IViewConfig.TupleSubscribe => tupleSubscribe;
        
        [Inject]
        public ViewConfig()
        {
            {
                var build = stringSubscribe = new SubscribeCollect<string>();
                build.Subscribe<VIEW_HIERARCY>("Assets/GAssets/RemoteGroup/View/ViewLayout.prefab");
            }

            {
                var build = tupleSubscribe = new SubscribeCollect<(string path, VIEW_HIERARCY hierarcy)>();
                build.Subscribe<HelloScreen>(("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame));
                build.Subscribe(VIEW.HelloScreen, ("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab", VIEW_HIERARCY.Frame));
            }
        }
    }
}

