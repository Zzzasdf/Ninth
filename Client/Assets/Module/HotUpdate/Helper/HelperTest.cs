using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class HelperTest : UIHelper
    {
        int asyncIndex = 0;
        int index = 0;
        public async void Awake()
        {
            Get<Button>("btnTest").onClick.AddListener(() => 1.Log());

            while(true)
            {
                await UniTask.Yield();
                await Update1();
            }
        }

        private async Task Update1()
        {
             (++asyncIndex).Log("异步{0}");
            Time.deltaTime.Log("异步{0}");
        }

        private void Update()
        {
            (++index).Log("同步{0}");
            Time.deltaTime.Log("同步{0}");
        }
    }
}