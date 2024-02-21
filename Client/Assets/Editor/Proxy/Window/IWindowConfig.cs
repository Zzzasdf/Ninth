using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Editor.Window
{
    public enum Tab
    {
        Build,
        Excel,
        Scan,
    }
    
    public interface IWindowConfig: IJson
    {
        T GetEnumType<T>() where T: Enum;
        void SetEnumType<T>(int value) where T : Enum;
        Dictionary<Type, int>.KeyCollection EnumTypeKeys();
        
        Type? Get(Tab tab);
        void Set(Tab key, Type value);
        Dictionary<Tab, Type?>.KeyCollection Keys();
    }
}