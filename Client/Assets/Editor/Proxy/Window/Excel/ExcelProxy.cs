using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ninth.Editor.Window;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class ExcelProxy: IExcelProxy
    {
        // private ReadOnlyDictionary<ExcelMode, Action> tabActionDic { get; } = new(new Dictionary<ExcelMode, Action>()
        // {
        //     { ExcelMode.Encode, new ExcelEncodeSettings().OnGUI },
        //     { ExcelMode.Search, new SearchSettings().OnGUI },
        // });
        //
        // private ExcelMode excelMode
        // {
        //     get => WindowSOCore.Get<SO>().ExcelMode;
        //     set => WindowSOCore.Get<SO>().ExcelMode = value;
        // }
        //
        // public void OnGUI()
        // {
        //     string[] barMenu = tabActionDic.Keys.ToArray().ToCurrLanguage();
        //     excelMode = (ExcelMode)GUILayout.Toolbar((int)excelMode, barMenu);
        //     if (tabActionDic.TryGetValue(excelMode, out Action action))
        //     {
        //         action?.Invoke();
        //     }
        // }
    }
}