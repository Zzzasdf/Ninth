using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public class ViewTest : IView
    {
        private GameObject node;
        private UIHelper uIHelper;

        public ActionHandler OnBtnLoginClicked;
        
        public ViewTest()
        {
            // GameDriver.AssetProxy.CloneAsync("");
            // UUHelper
        }

        public async UniTask Show()
        {
            uIHelper.Get<Button>("btnLogin")?.onClick.AddListener(OnBtnLoginClicked.OnTrigger);
        }

        public async UniTask Hide()
        {
            uIHelper.Get<Button>("btnLogin")?.onClick.RemoveListener(OnBtnLoginClicked.OnTrigger);
        }

        public void SetTxt(int a)
        {

        }
    }
}


