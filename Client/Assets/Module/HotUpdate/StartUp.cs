using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class StartUp : IAsyncStartable
    {
        private readonly IViewProxy viewProxy;
        
        [Inject]
        public StartUp(IViewProxy viewProxy)
        {
            this.viewProxy = viewProxy;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await viewProxy.Controller<LoginCtrl>().ShowView();
        }
    }
}
