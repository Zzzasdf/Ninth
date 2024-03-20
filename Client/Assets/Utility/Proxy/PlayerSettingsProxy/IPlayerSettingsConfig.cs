namespace Ninth.Utility
{
    public enum PLAY_SETTINGS
    {
        ProduceName,
        PlatformName,
    }

    public interface IPlayerSettingsConfig
    {
        SubscriberCollect<string, PLAY_SETTINGS> StringSubscriber { get; }
    }
}