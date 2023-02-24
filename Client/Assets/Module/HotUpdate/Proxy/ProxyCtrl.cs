using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public sealed class ProxyCtrl
    {
        public static ModelProxy ModelProxy { get; private set; }

        public static JsonProxy JsonProxy { get; private set; }
        static ProxyCtrl()
        {
            ModelProxy = new ModelProxy();

            JsonProxy = new JsonProxy();
        }
    }
}
