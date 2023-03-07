using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class ExcelSettings: EditorWindow
    {
        [MenuItem("Tools/Excels/Settings")]
        private static void PanelOpen()
        {
            GetWindow<ExcelSettings>();
        }

        private ExcelSettingsType m_ExcelSettingsType
        {
            get => (ExcelSettingsType)PlayerPrefsDefine.ExcelSettingsType;
            set => PlayerPrefsDefine.ExcelSettingsType = (int)value;
        }

        private void OnGUI()
        {
            string[] barMenu = new string[]
            {
                ExcelSettingsType.Encoder.ToString(),
                ExcelSettingsType.Search.ToString()
            };
            m_ExcelSettingsType = (ExcelSettingsType)GUILayout.Toolbar((int)m_ExcelSettingsType, barMenu);
            switch(m_ExcelSettingsType)
            {
                case ExcelSettingsType.Encoder:
                    {
                        break;
                    }
                case ExcelSettingsType.Search:
                    {
                        ExcelSearch.OnDraw();
                        break;
                    }
            }
        }
    }


    public enum ExcelSettingsType
    {
        Encoder,
        Search,
    }
}