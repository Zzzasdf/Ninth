namespace Ninth.HotUpdate
{
    public enum AssetStatus
    {
        Empty = 1 << 0,

        Loading = 1 << 1,

        Loaded = 1 << 2,
    }
}