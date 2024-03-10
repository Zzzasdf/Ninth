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
        SubscriberCollect<string> StringSubscriber { get; }
        SubscriberCollect<(string path, VIEW_HIERARCY hierarcy)> TupleSubscriber { get; }
    }
}
