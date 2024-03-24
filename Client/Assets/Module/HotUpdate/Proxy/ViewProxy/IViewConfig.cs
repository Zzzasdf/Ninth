using Ninth.Utility;

namespace Ninth.HotUpdate
{
    public enum VIEW_HIERARCHY
    {
        Bg,
        Main,
        Frame,
        Tip,
        Pop
    }

    public interface IViewConfig
    {
        SubscriberCollect<string> StringSubscriber { get; }
        SubscriberCollect<ViewConfig.ViewInfo> ViewInfoSubscriber { get; }
    }
}
