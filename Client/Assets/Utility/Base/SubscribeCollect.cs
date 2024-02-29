using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public class SubscribeCollect<TValue> : SubscribeCollect<TValue, Type, Enum>
    {
        
    }
    
    public class SubscribeCollect<TValue, TKeyGenerics, TKeyCommon>
        where TKeyCommon: Enum
    {
        private GenericsSubscribe<TKeyGenerics, TValue>? genericsSubscribe;
        private EnumTypeSubscribe<TValue>? enumTypeSubscribe;
        private CommonSubscribe<TKeyCommon, TValue>? commonSubscribe;

        #region Subcribe
        public ReactiveProperty<TValue> Subscribe<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand : TKeyGenerics
        {
            genericsSubscribe ??= new GenericsSubscribe<TKeyGenerics, TValue>();
            return genericsSubscribe.Subscribe<TKeyExpand>(value, markBit);
        }

        public ReactiveProperty<TValue> SubscribeByEnumType<TKeyEnum>(TValue value, int markBit = 0) where TKeyEnum : Enum
        {
            enumTypeSubscribe ??= new EnumTypeSubscribe<TValue>();
            return enumTypeSubscribe.Subscribe<TKeyEnum>(value, markBit);
        }

        public ReactiveProperty<TValue> Subscribe(TKeyCommon key, TValue value, int markBit = 0)
        {
            commonSubscribe ??= new CommonSubscribe<TKeyCommon, TValue>();
            return commonSubscribe.Subscribe(key, value, markBit);
        }
        #endregion

        #region Get
        public TValue? Get<TKeyExpand>(int markBit = 0) where TKeyExpand : TKeyGenerics
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKeyExpand)}".FrameError();
                return default;
            }
            return genericsSubscribe.Get<TKeyExpand>(markBit);
        }
        
        public TValue? GetByEnumType<TKeyEnum>(int markBit = 0) where TKeyEnum : Enum
        {
            if (enumTypeSubscribe == null)
            {
                $"未订阅 {typeof(TKeyEnum)}".FrameError();
                return default;
            }
            return enumTypeSubscribe.Get<TKeyEnum>(markBit);
        }

        public TValue? Get(TKeyCommon key, int markBit = 0)
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
        public void Set<TKeyExpand>(TValue value, int markBit = 0) where TKeyExpand : TKeyGenerics
        {
            if (genericsSubscribe == null)
            {
                $"未订阅 {typeof(TKeyExpand)}".FrameError();
                return;
            }
            genericsSubscribe.Set<TKeyExpand>(value, markBit);
        }

        public void SetByEnumType<TKeyEnum>(TValue value, int markBit = 0) where TKeyEnum : TKeyCommon
        {
            if (enumTypeSubscribe == null)
            {
                $"未订阅 {typeof(TKeyEnum)}".FrameError();
                return;
            }
            enumTypeSubscribe.Set<TKeyEnum>(value, markBit);
        }

        public void Set(TKeyCommon key, TValue value, int markBit = 0)
        {
            if (commonSubscribe == null)
            {
                $"未订阅 {key.GetType()}: {key}".FrameError();
                return;
            }
            commonSubscribe.Set(key, value, markBit);
        }
        #endregion

        #region TryGetValue
        public bool TryGetValue<TKeyExpand>(out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0) where TKeyExpand : class, TKeyGenerics
        {
            if (genericsSubscribe == null)
            {
                reactiveProperty = null;
                return false;
            }
            return genericsSubscribe.TryGetValue<TKeyExpand>(out reactiveProperty, markBit);
        }

        public bool TryGetValueByEnumType<TKeyEnum>(out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0) where TKeyEnum : Enum
        {
            if (enumTypeSubscribe == null)
            {
                reactiveProperty = null;
                return false;
            }
            return enumTypeSubscribe.TryGetValue<TKeyEnum>(out reactiveProperty, markBit);
        }

        public bool TryGetValue(TKeyCommon key, out ReactiveProperty<TValue>? reactiveProperty, int markBit = 0)
        {
            if (commonSubscribe == null)
            {
                reactiveProperty = null;
                return false;
            }
            return commonSubscribe.TryGetValue(key, out reactiveProperty, markBit);
        } 
        #endregion

        #region ContainsKey
        public bool ContainsKey<TKeyExpand>() where TKeyExpand : class, TKeyGenerics
        {
            if (genericsSubscribe == null)
            {
                return false;
            }
            return genericsSubscribe.ContainsKey<TKeyExpand>();
        }

        public bool ContainsKeyByEnumType<TKeyEnum>() where TKeyEnum : TKeyCommon
        {
            if (enumTypeSubscribe == null)
            {
                return false;
            }
            return enumTypeSubscribe.ContainsKey<TKeyEnum>();
        }

        public bool ContainsKey(TKeyCommon key)
        {
            if (commonSubscribe == null)
            {
                return false;
            }
            return commonSubscribe.ContainsKey(key);
        }
        #endregion

        #region Collection
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.KeyCollection? Keys() => genericsSubscribe?.Keys();
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.ValueCollection? Values() => genericsSubscribe?.Values();

        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.KeyCollection? KeysByEnumType() => enumTypeSubscribe?.Keys();
        public Dictionary<Type, LinkedListReactiveProperty<TValue>>.ValueCollection? ValuesByEnumType() => enumTypeSubscribe?.Values(); 

        public Dictionary<TKeyCommon, LinkedListReactiveProperty<TValue>>.KeyCollection? KeysByCommon() => commonSubscribe?.Keys();
        public Dictionary<TKeyCommon, LinkedListReactiveProperty<TValue>>.ValueCollection? ValuesByCommon() => commonSubscribe?.Values();
        #endregion
    }
}

