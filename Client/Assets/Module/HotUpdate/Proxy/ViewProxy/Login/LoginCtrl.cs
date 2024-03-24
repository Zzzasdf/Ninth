using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Ninth.HotUpdate
{
    public class LoginCtrl : IViewCtrl
    {
        private readonly IViewProxy viewProxy;
        private readonly LoginInputSystem loginInputSystem;

        private LoginView loginView;

        [Inject]
        public LoginCtrl(IViewProxy viewProxy, LoginInputSystem loginInputSystem)
        {
            this.viewProxy = viewProxy;
            this.loginInputSystem = loginInputSystem;
        }
        
        public async UniTask ShowView()
        {
            loginView = await viewProxy.ViewAsync<LoginView>();
            loginView.BtnStartGame.onClick.AddListener(OnBtnStartGameClick);
            loginView.BtnSettings.onClick.AddListener(UniTask.UnityAction(OnBtnSettingsClick));
            loginInputSystem.Menu.Any.performed += ctx =>
            {
                if (EventSystem.current.currentSelectedGameObject != null 
                    && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    return;
                EventSystem.current.SetSelectedGameObject(loginView.BtnStartGame.gameObject);
            };
            loginInputSystem.Menu.Enable();
        }
        
        public void CloseView()
        {
            EventSystem.current.SetSelectedGameObject(null);
            loginInputSystem.Menu.Disable();
        }
        
        private void OnBtnStartGameClick()
        {
            "TODO => StartGame".Log();
        }

        private async UniTaskVoid OnBtnSettingsClick()
        {
            await viewProxy.Controller<SettingsCtrl>().ShowView();
        }
    }
}
