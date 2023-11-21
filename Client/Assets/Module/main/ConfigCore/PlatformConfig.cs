using UnityEngine;

namespace Ninth
{
    public sealed class PlatformConfig
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName => Application.productName;

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName => Application.platform switch
        {
            RuntimePlatform.Android => "Android",
            RuntimePlatform.IPhonePlayer => "iOS",
            _ => "StandaloneWindows64"
        };
    }
}
