using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class ControllerTest
    {
        private ModelTest modelTest;
        private ViewTest viewTest;

        private void ViewTestRegister()
        {
            modelTest = new ModelTest();
            viewTest = new ViewTest();

            // View
            viewTest.OnBtnLoginClicked.OnRegister += OnLogin;
        }

        // ViewRegister
        private void OnLogin()
        {
            "登录".Log();
        }

        // UpdateData
        public void UpdateValue1(int a, string b)
        {
            SetTxt(a);
        }

        private void SetTxt(int a)
        {
            // Condition
            viewTest.SetTxt(a);
        }
    }
}

