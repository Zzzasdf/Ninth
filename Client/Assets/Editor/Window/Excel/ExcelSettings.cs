using Ninth.Editor.Excel.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class ExcelSettings
    {
        private ReadOnlyDictionary<ExcelMode, Action> tabActionDic { get; } = new(new Dictionary<ExcelMode, Action>()
        {
            { ExcelMode.Encode, new ExcelEncodeSettings().OnGUI },
            { ExcelMode.Search, new SearchSettings().OnGUI },
        });

        private ExcelMode excelMode
        {
            get => WindowSOCore.Get<WindowCollectConfig>().ExcelMode;
            set => WindowSOCore.Get<WindowCollectConfig>().ExcelMode = value;
        }

        public void OnGUI()
        {
            string[] barMenu = tabActionDic.Keys.ToArray().ToCurrLanguage();
            excelMode = (ExcelMode)GUILayout.Toolbar((int)excelMode, barMenu);
            if (tabActionDic.TryGetValue(excelMode, out Action action))
            {
                action?.Invoke();
            }
        }
    }
}