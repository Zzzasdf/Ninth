namespace Ninth.HotUpdate
{
    /// <summary>
    /// 日志验证器
    /// </summary>
    public enum LogValidator
    {
        /// <summary>
        /// 不生效
        /// </summary>
        None = 0,

        /// <summary>
        /// 框架
        /// </summary>
        FrameLog = 1 << 0,

        /// <summary>
        /// 框架警告
        /// </summary>
        FrameWarning = 1 << 1,

        /// <summary>
        /// 框架错误
        /// </summary>
        FrameError = 1 << 2,

        /// <summary>
        /// 普通
        /// </summary>
        Log = 1 << 3,

        /// <summary>
        /// 警告
        /// </summary>
        Warning = 1 << 4,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 1 << 5,

        /// <summary>
        /// 所有
        /// </summary>
        All = int.MaxValue
    }
}
