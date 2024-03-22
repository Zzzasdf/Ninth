using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class LoginCtrl : IViewCtrl
    {
        private readonly LoginModel loginModel;
        private readonly IViewProxy viewProxy;
        private readonly LoginInputSystem loginInputSystem;

        [Inject]
        public LoginCtrl(LoginModel loginModel, IViewProxy viewProxy, LoginInputSystem loginInputSystem)
        {
            this.loginModel = loginModel;
            this.viewProxy = viewProxy;
            this.loginInputSystem = loginInputSystem;
        }
        
        public async UniTask ShowView()
        {
            var loginView = await viewProxy.View<LoginView>();
            loginView.BtnContinue.onClick.AddListener(BtnContinueClick);
            loginView.BtnNewGame.onClick.AddListener(BtnNewGameClick);
            loginView.BtnLoadGame.onClick.AddListener(BtnLoadGameClick);
            loginView.BtnLibrary.onClick.AddListener(OnBtnLibraryClick);
            loginView.BtnSystem.onClick.AddListener(OnBtnSystemClick);
            loginInputSystem.LoginView.Any.performed += ctx =>
            {
                if (EventSystem.current.currentSelectedGameObject != null 
                    && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    return;
                EventSystem.current.SetSelectedGameObject(loginView.BtnContinue.gameObject);
            };
            loginInputSystem.LoginView.Enable();
        }
        
        public void CloseView()
        {
            EventSystem.current.SetSelectedGameObject(null);
            loginInputSystem.LoginView.Disable();
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
    }
}
