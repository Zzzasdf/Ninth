namespace Ninth.HotUpdate
{
    public enum BundleStatus
    {
        Empty = 1 << 0,

        Loading = 1 << 1,

        Loaded = 1 << 2,

        Unloading = 1 << 3,

        Unloaded = 1 << 4,
    }
}