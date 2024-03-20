using Ninth.Utility;

namespace Ninth
{
    public enum PLAYERPREFS_FLOAT
    {
        
    }
    
    public interface IPlayerPrefsFloatConfig
    {
        SubscriberCollect<float, PLAYERPREFS_FLOAT> FloatSubscriber { get; }
    }
}
