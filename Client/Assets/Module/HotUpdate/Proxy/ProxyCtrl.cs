using Cysharp.Threading.Tasks;
using System;

namespace Ninth.HotUpdate
{
    public partial class ProxyCtrl
    {
        // 静态常驻代理注册表
        public partial UniTask StaticResidentProxyRegister();


        // 静态泛型代理注册表
        // 清空注册
        public partial void StaticGenericProxyRegisterClear(Action clear);

        // 清空方法
        public partial void StaticGenericProxyClearAll();
    }
}
