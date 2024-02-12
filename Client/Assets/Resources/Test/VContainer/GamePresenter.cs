using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GamePresenter : IAsyncStartable
    {
        private readonly HelloWorldService helloWorldService;
        private readonly IViewProxy viewProxy;
        
        private HelloScreen? helloScreen;

        [Inject]
        public GamePresenter(HelloWorldService helloWorldService, IViewProxy viewProxy)
        {
            this.helloWorldService = helloWorldService;
            this.viewProxy = viewProxy;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            helloScreen = await viewProxy.Get<HelloScreen>(cancellation);
            if (helloScreen == null)
            {
                return;
            }
            helloScreen.HelloButton.onClick.AddListener(helloWorldService.Hello);
        }
    }
}