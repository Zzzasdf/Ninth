using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public class HelperTest : MonoBehaviour
    {
        private UIHelper helper;
        void Start()
        {
            helper = GetComponent<UIHelper>();
            helper.Get<Button>("Button (Legacy)").onClick.AddListener(() => 0.Log());
            helper.Get<Button>("Button (Legacy) (1)").onClick.AddListener(() => 1.Log());
            helper.Get<Button>("Button (Legacy) (2)").onClick.AddListener(() => 2.Log());
            helper.Get<Button>("Button (Legacy) (13)").onClick.AddListener(() => 2.Log());
        }

    }
}