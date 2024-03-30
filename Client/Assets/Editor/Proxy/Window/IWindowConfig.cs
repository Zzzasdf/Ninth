using System;
using Ninth.Utility;

namespace Ninth.Editor
{
    public enum Tab
    {
        Build,
        Module,
        Excel,
        Scan,
    }
    
    public interface IWindowConfig: IJson
    {
        MappingSelector<Tab, Type> MappingSelector { get; }
    }
}