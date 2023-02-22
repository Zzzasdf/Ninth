using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public partial class ProxyCtrl
    {
        public partial async UniTask StaticResidentProxyRegister()
        {
            await AssetProxy.Register();
        }
    }
}