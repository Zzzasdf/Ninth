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
            loginView = await viewProxy.ViewAsync<LoginView>();
            loginView.BtnStartGame.onClick.AddListener(OnBtnStartGameClick);
            loginView.BtnSettings.onClick.AddListener(UniTask.UnityAction(OnBtnSettingsClick));
            loginView.TxtPress.gameObject.SetActive(true);
            loginView.BtnStartGame.gameObject.SetActive(false);
            loginView.BtnSettings.gameObject.SetActive(false);
            loginInputSystem.Menu.Any.performed += ctx =>
            {
                if (EventSystem.current.currentSelectedGameObject != null 
                    && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    return;
                EventSystem.current.SetSelectedGameObject(loginView.BtnStartGame.gameObject);
                loginView.TxtPress.gameObject.SetActive(false);
                loginView.BtnStartGame.gameObject.SetActive(true);
                loginView.BtnSettings.gameObject.SetActive(true);
            };
            loginInputSystem.Menu.Enable();
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
