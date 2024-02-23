using System;
using System.Collections.Generic;
using Ninth.Utility;

namespace Ninth.Editor
{
    public interface IWindowProxy
    {
        T GetEnumType<T>() where T: Enum;
        void SetEnumType<T>(int value) where T : Enum;
        Dictionary<Type, ReactiveProperty<int>>.KeyCollection EnumTypeKeys();
        
        Type Get(Tab key);
        void Set(Tab key, Type value);
        Dictionary<Tab, ReactiveProperty<Type>>.KeyCollection Keys();
    }
}