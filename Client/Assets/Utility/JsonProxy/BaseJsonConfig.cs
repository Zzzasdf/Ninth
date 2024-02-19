using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public abstract class BaseJsonConfig: IJsonConfig
    {
        protected GenericsSubscribe<IJson, string?>? genericsSubscribe;
        protected EnumTypeSubscribe<string?>? enumTypeSubscribe;
        protected CommonSubscribe<Enum, string?>? commonSubscribe;
        
        string? IJsonConfig.Get<T>()
        {
            if (genericsSubscribe == null)
            {
                $"{genericsSubscribe} 未初始化".FrameError();
                return null;
            }
            return genericsSubscribe.Get<T>();
        }

        string? IJsonConfig.GetEnum<T>()
        {
            if (enumTypeSubscribe == null)
            {
                $"{enumTypeSubscribe} 未初始化".FrameError();
                return null;
            }
            return enumTypeSubscribe.Get<T>();
        }
        
        string? IJsonConfig.Get(Enum e)
        {
            if (commonSubscribe == null)
            {
                $"{commonSubscribe} 未初始化".FrameError();
                return null;
            }
            return commonSubscribe.Get(e);
        }
    }
}