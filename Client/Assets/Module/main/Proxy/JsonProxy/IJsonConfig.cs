using System.Collections.ObjectModel;

namespace Ninth
{
    public interface IJsonConfig
    {
        ReadOnlyDictionary<JsonFile, string> MapContainer();
    }
}
