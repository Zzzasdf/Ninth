using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public sealed class ProxyCtrl
    {
        public static JsonProxy JsonProxy { get; private set; }

        static ProxyCtrl()
        {
            JsonProxy = new JsonProxy();
        }
    }
}
