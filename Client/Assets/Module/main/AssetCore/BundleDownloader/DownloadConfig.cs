using System.Collections.Generic;

namespace Ninth
{
    public class DownloadConfig
    {
        public Dictionary<string, BundleInfo> BundleInfos;

        public DownloadConfig()
        {
            BundleInfos = new Dictionary<string, BundleInfo>();
        }
    }
}