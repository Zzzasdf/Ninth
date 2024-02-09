using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GamePresenter : IStartable
    {
        private readonly HelloWorldService helloWorldService;
        private readonly HelloScreen helloScreen;

        [Inject]
        public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
        {
            this.helloWorldService = helloWorldService;
            this.helloScreen = helloScreen;
        }

        public void Start()
        {
            helloScreen.HelloButton.onClick.AddListener(helloWorldService.Hello);
        }
    }
}