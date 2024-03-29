using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_FLOAT
    {
        
    }
    
    public interface IPlayerPrefsFloatConfig
    {
        Subscriber<PLAYERPREFS_FLOAT, float> FloatSubscriber { get; }
    }
}
