namespace Ninth.Utility
{
    public enum PLAY_SETTINGS
    {
        ProduceName,
        PlatformName,
    }

    public interface IPlayerSettingsConfig
    {
        Subscriber<PLAY_SETTINGS, string> StringSubscriber { get; }
    }
}