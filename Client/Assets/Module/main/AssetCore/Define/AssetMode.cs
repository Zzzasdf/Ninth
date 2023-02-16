/// <summary>
/// 资源模式
/// </summary>
namespace Ninth
{
    public enum AssetMode
    {
        NonAB = 1 << 0,

        LocalAB = 1 << 1,

        RemoteAB = 1 << 2,
    }
}