using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class LoginCtrl : IViewCtrl
    {
        private readonly LoginModel loginModel;
        private readonly IViewProxy viewProxy;

        [Inject]
        public LoginCtrl(LoginModel loginModel, IViewProxy viewProxy)
        {
            this.loginModel = loginModel;
            this.viewProxy = viewProxy;
        }
        
        public async UniTask ShowView()
        {
            var loginView = await viewProxy.View<LoginView>();
            loginView.BtnContinue.onClick.AddListener(BtnContinueClick);
            loginView.BtnNewGame.onClick.AddListener(BtnNewGameClick);
            loginView.BtnLoadGame.onClick.AddListener(BtnLoadGameClick);
            loginView.BtnLibrary.onClick.AddListener(OnBtnLibraryClick);
            loginView.BtnSystem.onClick.AddListener(OnBtnSystemClick);
            loginView.BtnExit.onClick.AddListener(OnBtnExitClick);
        }

        private void BtnContinueClick()
        {
            "TODO => Continue".Log();
        }

        private void BtnNewGameClick()
        {
            "TODO => NewGame".Log();
        }

        private void BtnLoadGameClick()
        {
            "TODO => LoadGame".Log();
        }

        private void OnBtnLibraryClick()
        {
            "TODO => Library".Log();
        }

        private void OnBtnSystemClick()
        {
            "TODO => System".Log();
        }

        private void OnBtnExitClick()
        {
            Application.Quit();
        }
    }
}
