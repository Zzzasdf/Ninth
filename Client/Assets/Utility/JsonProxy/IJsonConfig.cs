using System;
using System.Collections.ObjectModel;

namespace Ninth.Utility
{
    public interface IJsonConfig
    {
        string? Get<T>() where T : Type;
        string? Get(Enum e);
    }
}
