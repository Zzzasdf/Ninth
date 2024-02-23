using System;
using System.Collections.ObjectModel;
using Ninth.Utility;

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
        EnumTypeSubscribe<string> EnumTypeSubscribe { get; }
        GenericsSubscribe<IView, (string path, VIEW_HIERARCY hierarcy)> GenericsSubscribe { get; }
        CommonSubscribe<VIEW, (string path, VIEW_HIERARCY hierarchy)> CommonSubscribe { get; }
    }
}
