using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_INT
    {
        DownloadBundleStartPos,
    }
    
    public interface IPlayerPrefsIntConfig
    {
        Subscriber<PLAYERPREFS_INT, int> IntSubscriber { get; }
    }
}
