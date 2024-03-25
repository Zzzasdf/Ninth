using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class LoginCtrl : IViewCtrl
    {
        private readonly IViewProxy viewProxy;
        private readonly LoginInputSystem loginInputSystem;
        private readonly IObjectResolver resolver;

        private LoginView loginView;

        [Inject]
        public LoginCtrl(IViewProxy viewProxy, LoginInputSystem loginInputSystem, IObjectResolver resolver)
        {
            this.viewProxy = viewProxy;
            this.loginInputSystem = loginInputSystem;
            this.resolver = resolver;
        }
        
        public async UniTask ShowView()
        {
            "LoginView ..".Error();
            loginView = await viewProxy.ViewAsync<LoginView>();
            "LoginView2 ..".Error();
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
            "LoginView3 ..".Error();
        }
        
        public void CloseView()
        {
            EventSystem.current.SetSelectedGameObject(null);
            loginInputSystem.Menu.Disable();
            loginView.Recycle();
        }
        
        private void OnBtnStartGameClick()
        {
            "TODO => StartGame".Log();
        }

        private async UniTaskVoid OnBtnSettingsClick()
        {
            await resolver.Resolve<SettingsCtrl>().ShowView();
        }
    }
}
