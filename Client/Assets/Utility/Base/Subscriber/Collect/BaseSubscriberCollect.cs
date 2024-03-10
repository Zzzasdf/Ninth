using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public abstract class BaseSubscriberCollect<TValue, TEnum>
    {
        private GenericsSubscriber<TValue>? genericsSubscribe;
        private CommonSubscriber<TEnum, TValue>? commonSubscribe;
        
        #region Subcribe
        protected ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit)
        {
            genericsSubscribe ??= new GenericsSubscriber<TValue>();
            return genericsSubscribe.Container.Subscribe<TKey>(value, markBit);
        }

        protected ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit)
        {
            genericsSubscribe ??= new GenericsSubscriber<TValue>();
            return genericsSubscribe.FuncContainer.Subscribe<TKey>(valueFunc, markBit);
        }

        protected ReactiveProperty<TValue> Subscribe(TEnum key, TValue value, int markBit)
        {
            commonSubscribe ??= new CommonSubscriber<TEnum, TValue>();
            return commonSubscribe.Container.Subscribe(key, markBit, value);
        }
        
        protected ReactivePropertyFunc<TValue> Subscribe(TEnum key, Func<TValue> valueFunc, int markBit)
        {
            commonSubscribe ??= new CommonSubscriber<TEnum, TValue>();
            return commonSubscribe.FuncContainer.Subscribe(key, markBit, valueFunc);
        }
        #endregion

        #region Get
        protected TValue Get<TKey>(int markBit)
        {
            var reactiveProperty = GetReactiveProperty<TKey>(markBit);
            return reactiveProperty.Value;
        }
        
        protected ReactiveProperty<TValue> GetReactiveProperty<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKey)}".FrameError();
                return default!;
            }
            return genericsSubscribe.Container.Get<TKey>(markBit);
        }
        
        protected TValue GetByFunc<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKey)}".FrameError();
                return default!;
            }
            return genericsSubscribe.FuncContainer.Get<TKey>(markBit).Value;
        }
        
        protected TValue Get(TEnum key, int markBit)
        {
            var reactiveProperty = GetReactiveProperty(key, markBit);
            return reactiveProperty.Value;
        }

        protected ReactiveProperty<TValue> GetReactiveProperty(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return default!;
            }
            return commonSubscribe.Container.Get(key, markBit);
        }
        protected TValue GetByFunc(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return default!;
            }
            return commonSubscribe.Container.Get(key, markBit).Value;
        }
        #endregion

        #region Set
        protected void Set<TKey>(TValue value, int markBit)
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKey)}".FrameError();
                return;
            }
            genericsSubscribe.Container.Set<TKey>(markBit, value);
        }

        protected void Set(TEnum key, TValue value, int markBit)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return;
            }
            commonSubscribe.Container.Set(key, markBit, value);
        }
        #endregion

        #region TryGetValue
        protected bool TryGetValue<TKey>(out TValue? value, int markBit)
        {
            var result = TryGetReactivePropertyValue<TKey>(out var reactiveProperty, markBit);
            value = reactiveProperty != null ? reactiveProperty.Value : default;
            return result;
        }
        
        protected bool TryGetReactivePropertyValue<TKey>(out ReactiveProperty<TValue>? value, int markBit)
        {
            if (genericsSubscribe == null)
            {
                value = default;
                return false;
            }
            return genericsSubscribe.Container.TryGetValue<TKey>(markBit, out value);
        }
        
        protected bool TryGetValueByFunc<TKey>(out TValue? value, int markBit)
        {
            var result = TryGetReactivePropertyValueByFunc<TKey>(out var reactiveProperty, markBit);
            value = reactiveProperty != null ? reactiveProperty.Value : default;
            return result;
        }
        
        protected bool TryGetReactivePropertyValueByFunc<TKey>(out ReactivePropertyFunc<TValue>? value, int markBit)
        {
            if (genericsSubscribe == null)
            {
                value = default;
                return false;
            }
            return genericsSubscribe.FuncContainer.TryGetValue<TKey>(markBit, out value);
        }
        
        protected bool TryGetValue(TEnum key, out TValue? value, int markBit)
        {
            var result = TryGetReactivePropertyValue(key, out var reactiveProperty, markBit);
            value = reactiveProperty != null ? reactiveProperty.Value : default;
            return result;
        }

        protected bool TryGetReactivePropertyValue(TEnum key, out ReactiveProperty<TValue>? value, int markBit)
        {
            if (commonSubscribe == null)
            {
                value = default;
                return false;
            }
            return commonSubscribe.Container.TryGetValue(key, markBit, out value);
        } 
        
        protected bool TryGetValueByFunc(TEnum key, out TValue? value, int markBit)
        {
            var result = TryGetReactivePropertyValueByFunc(key, out var reactiveProperty, markBit);
            value = reactiveProperty != null ? reactiveProperty.Value : default;
            return result;
        }

        protected bool TryGetReactivePropertyValueByFunc(TEnum key, out ReactivePropertyFunc<TValue>? value, int markBit)
        {
            if (commonSubscribe == null)
            {
                value = default;
                return false;
            }
            return commonSubscribe.FuncContainer.TryGetValue(key, markBit, out value);
        } 
        #endregion

        #region ContainsKey
        protected bool ContainsKey<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                return false;
            }
            return genericsSubscribe.Container.ContainsKey<TKey>(markBit);
        }
        
        protected bool ContainsKeyByFunc<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                return false;
            }
            return genericsSubscribe.FuncContainer.ContainsKey<TKey>(markBit);
        }

        protected bool ContainsKey(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                return false;
            }
            return commonSubscribe.Container.ContainsKey(key, markBit);
        }
        
        protected bool ContainsKeyByFunc(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                return false;
            }
            return commonSubscribe.FuncContainer.ContainsKey(key, markBit);
        }
        #endregion

        #region Collection
        protected Dictionary<(Type type, int mariBit), ReactiveProperty<TValue>>.KeyCollection Keys()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.Container.Keys();
        }
        
        protected Dictionary<(Type type, int mariBit), ReactivePropertyFunc<TValue>>.KeyCollection KeysByFunc()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.FuncContainer.Keys();
        }

        protected Dictionary<(Type type, int mariBit), ReactiveProperty<TValue>>.ValueCollection Values()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.Container.Values();
        }

        protected Dictionary<(Type type, int mariBit), ReactivePropertyFunc<TValue>>.ValueCollection ValuesByFunc()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.FuncContainer.Values();
        }

        protected Dictionary<(TEnum key, int markBit), ReactiveProperty<TValue>>.KeyCollection KeysByCommon()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.Container.Keys();
        }
        
        protected Dictionary<(TEnum key, int markBit), ReactivePropertyFunc<TValue>>.KeyCollection KeysByCommonFunc()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.FuncContainer.Keys();
        }

        protected Dictionary<(TEnum key, int markBit), ReactiveProperty<TValue>>.ValueCollection ValuesByCommon()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.Container.Values();
        }
        
        protected Dictionary<(TEnum key, int markBit), ReactivePropertyFunc<TValue>>.ValueCollection ValuesByCommonFunc()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.FuncContainer.Values();
        }
        #endregion
    }
}

