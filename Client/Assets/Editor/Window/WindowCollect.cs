using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Ninth.Editor
{
    using ExcelSettingsData = Excel.ExcelSettingsData;

    public class WindowCollect : EditorWindow
    {
        [MenuItem("Tools/WindowCollect/Open")]
        private static void PanelOpen()
        {
            WindowCollect window = GetWindow<WindowCollect>();
            //window.position = new Rect(200, 200, 800, 500);
            window.position = new Rect(2200, 200, 1000, 700);
            window.splitterPos = 150;
        }

        [MenuItem("Tools/WindowCollect/Close")]
        private static void PanelClose()
        {
            GetWindow<WindowCollect>().Close();
        }

        private ReadOnlyDictionary<NinthWindowTab, Action> TabActionDic = new(new Dictionary<NinthWindowTab, Action>()
        {
             { NinthWindowTab.Build, new BuildSettings(EditorEntry.BuildAssetsCmd).OnGUI },
             { NinthWindowTab.Excel, new ExcelSettings<ExcelSettingsData>().OnGUI },
             { NinthWindowTab.Scan, new ScanSettings().OnGUI },
             { NinthWindowTab.Review, new ReviewSettings().OnGUI },
             { NinthWindowTab.Other, new OtherSettings<OtherSettingsData>().OnGUI },
        });

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
        private Vector2 contentSrollView;

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                RenderLeftTag();
                RenderSplitter();
                RenderRightContent();
            }
        }

        private void RenderLeftTag()
        {
            // 页签
            tabScrollView = EditorGUILayout.BeginScrollView(tabScrollView,
                GUILayout.Width(splitterPos),
                GUILayout.MaxWidth(splitterPos),
                GUILayout.MinWidth(splitterPos));

            string[] barMenu = TabActionDic.Keys.Select(x => x.ToString()).ToArray();
            NinthWindowTab = (NinthWindowTab)GUILayout.SelectionGrid((int)NinthWindowTab, barMenu, 1);
            EditorGUILayout.EndScrollView();
        }

        private void RenderSplitter()
        {
            // 分割线
            GUILayout.Box("",
                GUILayout.Width(splitterWidth),
                GUILayout.MaxWidth(splitterWidth),
                GUILayout.MinWidth(splitterWidth),
                GUILayout.ExpandHeight(true));
            splitterRect = GUILayoutUtility.GetLastRect();
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

        private void RenderRightContent()
        {
            // 页签内容
            contentSrollView = EditorGUILayout.BeginScrollView(contentSrollView, GUILayout.ExpandWidth(true));
            TabActionDic[NinthWindowTab]?.Invoke();
            EditorGUILayout.EndScrollView();
        }
    }

    public enum NinthWindowTab
    {
        Build,
        Excel,
        Scan,
        Review,
        Other
    }
}