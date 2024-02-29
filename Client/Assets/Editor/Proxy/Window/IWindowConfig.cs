using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Editor
{
    public enum Tab
    {
        Build,
        Excel,
        Scan,
    }
    
    public interface IWindowConfig: IJson
    {
        SubscribeCollect<int> IntSubscribe { get; }
        SubscribeCollect<Type, Tab> TypeSubscribe { get; }
    }
}