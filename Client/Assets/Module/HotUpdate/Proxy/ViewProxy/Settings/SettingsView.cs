using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class SettingsView : BaseView
    {
        public Button BtnAudio;
        public Button BtnSaveData;
        public Button BtnDisplay;
        public Button BtnLanguage;
        public Button BtnPrivacy;

        private void Awake()
        {
            var btns = GetComponentsInChildren<Button>().ToDictionary(value => value.name, value => value);
            BtnAudio = btns["btnAudio"];
            BtnSaveData = btns["btnSaveData"];
            BtnDisplay = btns["btnDisplay"];
            BtnLanguage = btns["btnLanguage"];
            BtnPrivacy = btns["btnPrivacy"];
        }
    }
}
