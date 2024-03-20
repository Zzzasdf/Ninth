using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_INT
    {
        DownloadBundleStartPos,
    }
    
    public interface IPlayerPrefsIntConfig
    {
        SubscriberCollect<int, PLAYERPREFS_INT> IntSubscriber { get; }
    }
}
