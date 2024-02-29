using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public abstract class BaseSubscribeCollect<TValue, TEnum>
    {
        private GenericsSubscribe<TValue>? genericsSubscribe;
        private CommonSubscribe<TEnum, TValue>? commonSubscribe;
        
        #region Subcribe
        protected ReactiveProperty<TValue> Subscribe<TKey>(TValue value, int markBit)
        {
            genericsSubscribe ??= new GenericsSubscribe<TValue>();
            return genericsSubscribe.Subscribe<TKey>(value, markBit);
        }

        protected ReactivePropertyFunc<TValue> Subscribe<TKey>(Func<TValue> valueFunc, int markBit)
        {
            genericsSubscribe ??= new GenericsSubscribe<TValue>();
            return genericsSubscribe.Subscribe<TKey>(valueFunc, markBit);
        }

        protected ReactiveProperty<TValue> Subscribe(TEnum key, TValue value, int markBit)
        {
            commonSubscribe ??= new CommonSubscribe<TEnum, TValue>();
            return commonSubscribe.Subscribe(key, markBit, value);
        }
        
        protected ReactivePropertyFunc<TValue> Subscribe(TEnum key, Func<TValue> valueFunc, int markBit)
        {
            commonSubscribe ??= new CommonSubscribe<TEnum, TValue>();
            return commonSubscribe.Subscribe(key, markBit, valueFunc);
        }
        #endregion

        #region Get
        protected TValue Get<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKey)}".FrameError();
                return default;
            }
            return genericsSubscribe.Get<TKey>(markBit);
        }

        protected TValue Get(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return default;
            }
            return commonSubscribe.Get(key, markBit);
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
            genericsSubscribe.Set<TKey>(markBit, value);
        }

        protected void Set(TEnum key, TValue value, int markBit)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return;
            }
            commonSubscribe.Set(key, markBit, value);
        }
        #endregion

        #region TryGetValue
        protected bool TryGetValue<TKey>(out TValue value, int markBit)
        {
            if (genericsSubscribe == null)
            {
                value = default;
                return false;
            }
            return genericsSubscribe.TryGetValue<TKey>(markBit, out value);
        }

        protected bool TryGetValue(TEnum key, out TValue value, int markBit)
        {
            if (commonSubscribe == null)
            {
                value = default;
                return false;
            }
            return commonSubscribe.TryGetValue(key, markBit, out value);
        } 
        #endregion

        #region ContainsKey
        protected bool ContainsKey<TKey>(int markBit)
        {
            if (genericsSubscribe == null)
            {
                return false;
            }
            return genericsSubscribe.ContainsKey<TKey>(markBit);
        }

        protected bool ContainsKey(TEnum key, int markBit)
        {
            if (commonSubscribe == null)
            {
                return false;
            }
            return commonSubscribe.ContainsKey(key, markBit);
        }
        #endregion

        #region Collection
        protected Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.KeyCollection Keys()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.Keys();
        }

        protected Dictionary<(Type type, int mariBit), IReactiveProperty<TValue>>.ValueCollection Values()
        {
            if (genericsSubscribe == null)
            {
                $"该集合未订阅内容 {genericsSubscribe}".FrameError();
                return default;
            }
            return genericsSubscribe.Values();
        }

        protected Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.KeyCollection KeysByCommon()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.Keys();
        }

        protected Dictionary<(TEnum key, int markBit), IReactiveProperty<TValue>>.ValueCollection ValuesByCommon()
        {
            if (commonSubscribe == null)
            {
                $"该集合未订阅内容 {commonSubscribe}".FrameError();
                return default;
            }
            return commonSubscribe.Values();
        }
        #endregion
    }
}

