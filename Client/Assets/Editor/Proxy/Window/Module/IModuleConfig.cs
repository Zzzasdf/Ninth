using System;
using Ninth.Utility;

namespace Ninth.Editor
{
    public enum ModuleTab
    {
        View,
    }
    
    public interface IModuleConfig
    {
        MappingSelector<ModuleTab, Type> MappingSelector { get; }
    }
}
