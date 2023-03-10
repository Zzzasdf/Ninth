using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public sealed class ProxyCtrl
    {
        public static ModelProxy ModelProxy { get; private set; }
        public static DownloaderProxy DownloaderProxy { get; private set; }

        static ProxyCtrl()
        {
            ModelProxy = new ModelProxy();
            DownloaderProxy = new DownloaderProxy();
        }
    }
}
