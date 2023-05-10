using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class ExcelSettings: EditorWindow
    {
        [MenuItem("Tools/ExcelSettings")]
        private static void PanelOpen()
        {
            GetWindow<ExcelSettings>();
        }
        private Dictionary<ExcelMode, Action> cache;
        private Dictionary<ExcelMode, Action> Cache
        {
            get
            {
                if(cache == null)
                {
                    cache = new Dictionary<ExcelMode, Action>();
                    cache.Add(ExcelMode.Encode, new ExcelEncode().OnDraw);
                    cache.Add(ExcelMode.Search, new ExcelSearch().OnDraw);
                }
                return cache;
            }
        }

        private ExcelMode ExcelMode
        {
            get => EditorSOCore.GetExcelConfig().ExcelMode;
            set => EditorSOCore.GetExcelConfig().ExcelMode = value;
        }

        private void OnGUI()
        {
            string[] barMenu = Cache.Keys.Select(x => x.ToString()).ToArray();
            ExcelMode = (ExcelMode)GUILayout.Toolbar((int)ExcelMode, barMenu);
            if(Cache.TryGetValue(ExcelMode, out Action action))
            {
                action?.Invoke();
            }
        }
    }
}