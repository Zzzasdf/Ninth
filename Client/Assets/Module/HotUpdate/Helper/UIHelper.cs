using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public enum UIHelperBarMode
    {
        Button = 1 << 0,
        Text = 1 << 1,
    }

    public class UIHelper : MonoBehaviour
    {
        public int m_Lock;
        public UIHelperBarMode m_UIHelperBarMode;

        public List<UIHelperBarMode> m_UIHelperBarModes;
        public int m_UIHelperBarModeIndex;

        public List<UIHelperBarMode> m_DataBarModes;
        public List<KeyList> m_Keys;
        public List<ValueList> m_Values;
    }

    [System.Serializable]
    public class KeyList
    {
        public List<string> Values;
        public KeyList()
        {
            Values = new List<string>();
        }
    }

    [System.Serializable]
    public class ValueList
    {
        public List<Object> Values;
        public ValueList()
        {
            Values = new List<Object>();
        }
    }
}