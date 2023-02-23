using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public sealed partial class ProxyCtrl
    {
        public static JsonProxy JsonProxy;

        static ProxyCtrl()
        {
            JsonProxy = new JsonProxy();
        }
    }
}
