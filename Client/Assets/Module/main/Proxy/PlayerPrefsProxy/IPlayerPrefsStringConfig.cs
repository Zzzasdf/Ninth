using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_STRING
    {
        DownloadBundleStartPosFromAssetVersion,
    }
    
    public interface IPlayerPrefsStringConfig
    {
        SubscriberCollect<string, PLAYERPREFS_STRING> StringSubscriber { get; }
    }
}
