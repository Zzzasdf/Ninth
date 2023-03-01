using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public sealed class ProxyCtrl
    {
        public static ModelProxy ModelProxy { get; private set; }

        static ProxyCtrl()
        {
            ModelProxy = new ModelProxy();
        }
    }
}
