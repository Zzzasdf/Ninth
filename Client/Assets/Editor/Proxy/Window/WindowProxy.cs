using System;
using System.Collections.Generic;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class WindowProxy: IWindowProxy
    {
        private readonly IWindowConfig windowConfig;
        private readonly IJsonProxy jsonProxy;
        
        [Inject]
        public WindowProxy(IWindowConfig windowConfig, IJsonProxy jsonProxy)
        {
            this.windowConfig = windowConfig;
            this.jsonProxy = jsonProxy;
        }
        
        T IWindowProxy.GetEnumType<T>()
        {
            return (T)Enum.ToObject(typeof(T),windowConfig.EnumTypeSubscribe.Get<T>());
        }

        void IWindowProxy.SetEnumType<T>(int value)
        {
            windowConfig.EnumTypeSubscribe.Set<T>(value);
        }

        Dictionary<Type, ReactiveProperty<int>>.KeyCollection IWindowProxy.EnumTypeKeys()
        {
            return windowConfig.EnumTypeSubscribe.Keys();
        }

        Type IWindowProxy.Get(Tab key)
        {
            return windowConfig.CommonSubscribe.Get(key);
        }

        void IWindowProxy.Set(Tab key, Type value)
        {
            windowConfig.CommonSubscribe.Set(key, value);
        }

        Dictionary<Tab, ReactiveProperty<Type>>.KeyCollection IWindowProxy.Keys()
        {
            return windowConfig.CommonSubscribe.Keys();
        }
    }
}