using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public abstract class BaseJsonConfig: IJsonConfig
    {
        protected GenericsSubscribe<Type, string?>? typeSubscribe;
        protected BaseSubscribe<Enum, string?>? baseSubscribe;
        
        string? IJsonConfig.Get(Enum e)
        {
            if (baseSubscribe == null)
            {
                $"{baseSubscribe} 未初始化".FrameError();
                return null;
            }
            return baseSubscribe.Get(e);
        }

        string? IJsonConfig.Get<T>() 
        {
            if (typeSubscribe == null)
            {
                $"{typeSubscribe} 未初始化".FrameError();
                return null;
            }
            return typeSubscribe.Get<T>();
        }
    }
}