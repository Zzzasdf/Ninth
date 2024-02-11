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
        private HelloWorldService helloWorldService;
        
        private IAssetProxy assetProxy;
        private HelloScreen helloScreen;

        [Inject]
        public GamePresenter(HelloWorldService helloWorldService, IAssetProxy assetProxy)
        {
            111.Log();
            this.helloWorldService = helloWorldService;
            this.assetProxy = assetProxy;
            222.Log();
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            333.Log();
            helloScreen = await assetProxy.ViewLoadAsync<HelloScreen>(cancellation);
            helloScreen.HelloButton.onClick.AddListener(helloWorldService.Hello);
            444.Log();
        }
    }
}