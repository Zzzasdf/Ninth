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

        private ExcelSettingsType ExcelSettingsType
        {
            get => EditorSOCore.GetExcelConfig().ExcelSettingsType;
            set => EditorSOCore.GetExcelConfig().ExcelSettingsType = value;
        }

        private void OnGUI()
        {
            string[] barMenu = new string[]
            {
                ExcelSettingsType.Encoder.ToString(),
                ExcelSettingsType.Search.ToString()
            };
            ExcelSettingsType = (ExcelSettingsType)GUILayout.Toolbar((int)ExcelSettingsType, barMenu);
            switch(ExcelSettingsType)
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
}