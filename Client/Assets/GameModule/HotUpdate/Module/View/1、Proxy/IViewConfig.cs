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
        Subscriber<string, string> LayoutSubscriber { get; }
        Subscriber<string, ViewConfig.ViewInfo> ViewInfoSubscriber { get; }
    }
}
