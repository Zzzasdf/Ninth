using System;
using Ninth.Utility;

namespace Ninth.Editor
{
    public enum AssetModuleTab
    {
        View,
        Role,
    }
    
    public interface IAssetModuleConfig
    {
        MappingSelector<AssetModuleTab, Type> MappingSelector { get; }
    }
}
