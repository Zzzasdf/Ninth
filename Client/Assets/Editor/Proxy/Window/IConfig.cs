using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth.Editor.Window
{
    
    public interface IConfig
    {
        Tab CurrentTab { get; set; }
        Type? Get(Tab tab);
        void Set(Tab tab);
        ReadOnlyDictionary<Tab, Type?>.KeyCollection Keys { get; }
        ReadOnlyDictionary<Tab, Type?>.ValueCollection Values { get; }
        void Save();
    }
}
