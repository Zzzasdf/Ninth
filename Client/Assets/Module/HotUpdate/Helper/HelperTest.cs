using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class HelperTest : UIHelper
    {
        public void Awake()
        {
            Get<Button>("btnTest").onClick.AddListener(() => 1.Log());
        }

        public void Update()
        {

        }
    }
}