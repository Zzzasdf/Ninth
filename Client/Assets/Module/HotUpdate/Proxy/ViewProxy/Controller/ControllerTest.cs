using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Ninth.HotUpdate
{
    public sealed partial class ControllerTest : IController
    {
        public ControllerTest()
        {
            ViewTestRegister();
        }

        public async UniTask ShowViewTest()
        {
            await viewTest.Show();
        }

        public async UniTask HideViewTest()
        {
            await viewTest.Hide();
        }
    }
}