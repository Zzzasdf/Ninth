using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Ninth.HotUpdate
{
    public class SettingsCtrl : IViewCtrl
    {
        private readonly SettingsModel settingsModel;
        private readonly IViewProxy viewProxy;
        private readonly SettingsInputSystem settingsInputSystem;

        private SettingsView settingsView;
        
        [Inject]
        public SettingsCtrl(SettingsModel settingsModel, IViewProxy viewProxy, SettingsInputSystem settingsInputSystem)
        {
            this.settingsModel = settingsModel;
            this.viewProxy = viewProxy;
            this.settingsInputSystem = settingsInputSystem;
        }

        public async UniTask ShowView()
        {
            settingsView = await viewProxy.ViewAsync<SettingsView>();
            settingsView.BtnAudio.onClick.AddListener(OnBtnAudioClick);
            settingsView.BtnSaveData.onClick.AddListener(OnBtnSaveDataClick);
            settingsView.BtnDisplay.onClick.AddListener(OnBtnDisplayClick);
            settingsView.BtnLanguage.onClick.AddListener(OnBtnLanguageClick);
            settingsView.BtnPrivacy.onClick.AddListener(OnBtnPrivacyClick);
            settingsInputSystem.Menu.Any.performed += ctx =>
            {
                if (EventSystem.current.currentSelectedGameObject != null 
                    && EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                    return;
                EventSystem.current.SetSelectedGameObject(settingsView.BtnAudio.gameObject);
            };
            settingsInputSystem.Menu.Back.performed += ctx => CloseView();
            EventSystem.current.SetSelectedGameObject(settingsView.BtnAudio.gameObject);
            settingsInputSystem.Menu.Enable();
        }

        public void CloseView()
        {
            EventSystem.current.SetSelectedGameObject(null);
            settingsInputSystem.Menu.Disable();
            settingsView.Recycle();
        }

        private void OnBtnAudioClick()
        {
            "TODO => Audio".Log();
        }

        private void OnBtnSaveDataClick()
        {
            "TODO => Save Data".Log();
        }

        private void OnBtnDisplayClick()
        {
            "TODO => Display".Log();
        }

        private void OnBtnLanguageClick()
        {
            "TODO => Language".Log();
        }

        private void OnBtnPrivacyClick()
        {
            "TODO => Privacy".Log();
        }
    }
}
