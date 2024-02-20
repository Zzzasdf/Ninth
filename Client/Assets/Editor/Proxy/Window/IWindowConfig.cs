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
        Tab CurrentTab { get; set; }
        string? GetEnumType<T>() where T: Enum;
        (Type type, string path)? Get(Tab tab);
        Dictionary<Type, string?>.KeyCollection EnumTypeKeys();
        Dictionary<Tab, (Type type, string path)?>.KeyCollection Keys();
    }
}