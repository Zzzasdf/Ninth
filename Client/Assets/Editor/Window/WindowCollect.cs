using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace Ninth.Editor
{
    using ExcelSettingsData = Excel.ExcelSettingsData;

    public class WindowCollect : EditorWindow
    {
        [MenuItem("Tools/WindowCollect")]
        private static void PanelOpen()
        {
            WindowCollect window = GetWindow<WindowCollect>();
            window.position = new Rect(200, 200, 800, 500);
            window.splitterPos = 150;
        }

        private Dictionary<NinthWindowTab, Action> tabActionDic = new Dictionary<NinthWindowTab, Action>()
        {
            [NinthWindowTab.Excel] = new ExcelSettings<ExcelSettingsData>().OnGUI,
            [NinthWindowTab.Scan] = new ScanSettings().OnGUI,
            [NinthWindowTab.Build] = new BuildSettings().OnGUI,
            [NinthWindowTab.Other] = new OtherSettings<OtherSettingsData>().OnGUI,
        };

        private NinthWindowTab NinthWindowTab
        {
            get => WindowSOCore.Get<WindowCollectConfig>().NinthWindowTab;
            set => WindowSOCore.Get<WindowCollectConfig>().NinthWindowTab = value;
        }

        // 页签
        private Vector2 tabScrollView;

        // 分割线
        private float splitterPos;
        private float splitterWidth = 5;
        private Rect splitterRect;
        private bool dragging;

        // 页签内容
        private Vector2 actionSrollView;

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            // 页签
            tabScrollView = EditorGUILayout.BeginScrollView(tabScrollView,
                GUILayout.Width(splitterPos),
                GUILayout.MaxWidth(splitterPos),
                GUILayout.MinWidth(splitterPos));

            string[] barMenu = tabActionDic.Keys.Select(x => x.ToString()).ToArray();
            NinthWindowTab = (NinthWindowTab)GUILayout.SelectionGrid((int)NinthWindowTab, barMenu, 1);
            EditorGUILayout.EndScrollView();

            // 分割线
            GUILayout.Box("",
                GUILayout.Width(splitterWidth),
                GUILayout.MaxWidth(splitterWidth),
                GUILayout.MinWidth(splitterWidth),
                GUILayout.ExpandHeight(true));
            splitterRect = GUILayoutUtility.GetLastRect();

            // 页签内容
            actionSrollView = EditorGUILayout.BeginScrollView(actionSrollView, GUILayout.ExpandWidth(true));
            tabActionDic[NinthWindowTab]?.Invoke();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            // 分割线事件
            if (Event.current != null)
            {
                switch (Event.current.rawType)
                {
                    case EventType.MouseDown:
                        if (splitterRect.Contains(Event.current.mousePosition))
                        {
                            dragging = true;
                        }
                        break;
                    case EventType.MouseDrag:
                        if (dragging)
                        {
                            splitterPos += Event.current.delta.x;
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        if (dragging)
                        {
                            dragging = false;
                        }
                        break;
                }
            }
        }
    }

    public enum NinthWindowTab
    {
        Excel,
        Scan,
        Build,
        Other
    }
}