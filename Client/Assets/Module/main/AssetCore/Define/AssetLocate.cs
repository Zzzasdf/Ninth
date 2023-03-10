namespace Ninth
{
    /// <summary>
    /// 资源定位
    /// </summary>
    public enum AssetLocate
    {
        /// <summary>
        /// 本地
        /// </summary>
        Local = 1 << 0,

        /// <summary>
        /// 远端
        /// </summary>
        Remote = 1 << 1,

        /// <summary>
        /// 程序集
        /// </summary>
        Dll = 1 << 2,

        All = int.MaxValue
    }
}