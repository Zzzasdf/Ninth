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
        SubscribeCollect<string> StringSubscribe { get; }
        SubscribeCollect<(string path, VIEW_HIERARCY hierarcy)> TupleSubscribe { get; }
    }
}
