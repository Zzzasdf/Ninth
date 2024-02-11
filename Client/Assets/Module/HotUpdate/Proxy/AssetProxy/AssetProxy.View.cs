using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public interface IView { }

    public partial class AssetProxy
    {
        private ReadOnlyDictionary<Type, string> viewPathMap;

        private void ViewSubscribe()
        {
            var contianer = new Dictionary<Type, string>();
            Subscribe<HelloScreen>("Assets/GAssets/RemoteGroup/View/Test/HelloScreen.prefab");
            viewPathMap = new(contianer);

            void Subscribe<T>(string path) where T: IView
            {
                Type type = typeof(T);
                if (!contianer.TryAdd(type, path))
                {
                    type.FrameError("重复注册 View: {0}");
                }
            }
        }

        public async UniTask<T> ViewLoadAsync<T>(CancellationToken cancellationToken = default) where T: IView
        {
            Type type = typeof(T);
            viewPathMap.Count.Log("数量{0}");
            if (!viewPathMap.TryGetValue(type, out var path))
            {
                type.FrameError("未注册 View: {0}");
            }
            GameObject view = await CloneAsync(path!, cancellationToken);
            return view.GetComponent<T>();
            // UnityEngine.Object prefab = await Resources.LoadAsync(path);
            // UnityEngine.GameObject obj = UnityEngine.Object.Instantiate(prefab) as GameObject;
            // return obj.GetComponent<T>();
        }
    }
}

