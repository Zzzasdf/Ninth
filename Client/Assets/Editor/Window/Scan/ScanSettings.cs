using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace Ninth.Editor
{
    public class ScanSettings : EditorWindow
    {
        [MenuItem("Tools/ScanSettings")]
        private static void PanelOpen()
        {
            GetWindow<ScanSettings>();
        }
        private Dictionary<ScanMode, Action> cache;
        private Dictionary<ScanMode, Action> Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new Dictionary<ScanMode, Action>();
                    cache.Add(ScanMode.Prefab, new ScanPrefabSettings().OnDraw);
                    cache.Add(ScanMode.Others, null);
                }
                return cache;
            }
        }

        private ScanMode ScanMode
        {
            get => EditorSOCore.GetScanConfig().ScanMode;
            set => EditorSOCore.GetScanConfig().ScanMode = value;
        }

        private void OnGUI()
        {
            string[] barMenu = Cache.Keys.Select(x => x.ToString()).ToArray();
            ScanMode = (ScanMode)GUILayout.Toolbar((int)ScanMode, barMenu);
            if (Cache.TryGetValue(ScanMode, out Action action))
            {
                action?.Invoke();
            }
        }
    }
}
