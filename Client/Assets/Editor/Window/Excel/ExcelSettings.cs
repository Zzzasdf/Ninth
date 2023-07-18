using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class ExcelSettings<T>
        where T : IExcelSettingsData, new()
    {
        private T data;

        public ExcelSettings()
        {
            data = new T();
        }

        public void OnGUI()
        {
            string[] barMenu = data.TabDic.Keys.Select(x => x.ToString()).ToArray();
            data.ExcelMode = (ExcelMode)GUILayout.Toolbar((int)data.ExcelMode, barMenu);
            if (data.TabDic.TryGetValue(data.ExcelMode, out Action action))
            {
                action?.Invoke();
            }
        }
    }
}