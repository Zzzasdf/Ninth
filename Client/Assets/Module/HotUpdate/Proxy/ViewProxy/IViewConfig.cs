using System;
using System.Collections.ObjectModel;

namespace Ninth.HotUpdate
{
    public enum VIEW_HIERARCY
    {
        Bg,
        Main,
        Frame,
        Tip,
        Pop
    }

    public enum VIEW
    {
        HelloScreen
    }

    public interface IViewConfig
    {
        string ViewLayoutPath();
        (string? path, VIEW_HIERARCY? hierarchy)? Get<T>() where T: class, IView;
        (string? path, VIEW_HIERARCY? hierarchy)? Get(VIEW view);
    }
}
