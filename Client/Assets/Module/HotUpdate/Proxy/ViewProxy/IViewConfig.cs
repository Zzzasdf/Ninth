using System;
using System.Collections.ObjectModel;

namespace Ninth.HotUpdate
{
    public enum ViewHierarchy
    {
        Bg,
        Main,
        Frame,
        Tip,
        Pop
    }

    public interface IViewConfig
    {
        string ViewLayoutPath();
        ReadOnlyDictionary<Type, (string path, ViewHierarchy hierarchy)> MapContainer();
    }
}
