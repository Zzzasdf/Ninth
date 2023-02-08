using UnityEngine;

namespace Ninth
{
    public class PlatformConfig
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public static string ProduceName { get; } = Application.productName;

        /// <summary>
        /// 平台名称
        /// </summary>
        public static string PlatformName { get; } =
            Application.platform switch
            {
                RuntimePlatform.Android => "Android",
                RuntimePlatform.IPhonePlayer => "iOS",
                _ => "StandaloneWindows64"
            };
    }
}
