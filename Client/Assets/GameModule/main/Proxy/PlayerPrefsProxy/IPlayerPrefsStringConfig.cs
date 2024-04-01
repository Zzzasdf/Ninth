using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_STRING
    {
        DownloadBundleStartPosFromAssetVersion,
    }
    
    public interface IPlayerPrefsStringConfig
    {
        Subscriber<PLAYERPREFS_STRING, string> StringSubscriber { get; }
    }
}
