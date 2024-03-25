using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class LoginView : BaseView
    {
        public Button BtnStartGame; 
        public Button BtnSettings;
        
        // private void Awake()
        // {
        //     var btns = GetComponentsInChildren<Button>().ToDictionary(value => value.name, value => value);
        //     BtnStartGame = btns["btnStartGame"];
        //     BtnSettings = btns["btnSettings"];
        // }
    }
}
