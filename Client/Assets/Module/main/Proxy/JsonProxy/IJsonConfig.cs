using System;
using System.Collections.ObjectModel;

namespace Ninth
{
    public interface IJsonConfig
    {
        string? Get(Enum e);
    }
}
