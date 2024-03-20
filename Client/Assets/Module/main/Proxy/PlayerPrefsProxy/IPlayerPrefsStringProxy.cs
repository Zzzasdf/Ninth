namespace Ninth
{
    public interface IPlayerPrefsStringProxy
    {
        string Get(PLAYERPREFS_STRING playerprefsString);

        void Set(PLAYERPREFS_STRING playerprefsString, string value);
    }
}
