using System;
using System.Collections.ObjectModel;

namespace Ninth.Utility
{
    public interface IJson
    {
        
    }
    
    public interface IJsonConfig
    {
        string? Get<T>() where T : IJson;
        string? GetEnum<T>() where T: Enum;
        string? Get(Enum e);
    }
}
