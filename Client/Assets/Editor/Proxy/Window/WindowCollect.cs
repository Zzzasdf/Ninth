using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ninth.Editor.Window;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor.Window
{
    public class WindowCollect : EditorWindow
    {
        private static WindowCollect window; 
        [MenuItem("Tools/WindowCollect/Open %w")]
        private static void PanelOpen()
        {
            window = GetWindow<WindowCollect>();
            // window.position = new Rect(200, 200, 800, 500);
            window.position = new Rect(2200, 200, 1000, 700);
            window.splitterPos = 150;
        }

        [MenuItem("Tools/WindowCollect/Close %g")]
        private static void PanelClose()
        {
            window.Close();
        }

        [MenuItem("Tools/WindowCollect/Display")]
        private static void Display()
        {
            EditorUtility.DisplayDialog("11", "cancel", "ok");
        }

        public static void SubscribeResolver(IObjectResolver resolver)
        {
            WindowCollect.resolver = resolver;
        }
        private static IObjectResolver resolver;
        private IJsonProxy jsonProxy;
        private IWindowProxy windowProxy;

        private void OnEnable()
        {
            jsonProxy = resolver.Resolve<IJsonProxy>();
            windowProxy = resolver.Resolve<IWindowProxy>();
        }

        private void OnDisable()
        {
            if (jsonProxy.CacheExists<BuildJson>(Tab.Build))
            {
                jsonProxy.ToJson<BuildJson>(Tab.Build);
            }
            AssetDatabase.Refresh();
        }
        
        private void OnGUI()
        {
            using var horizontalScope = new GUILayout.HorizontalScope();
            RenderTab();
            RenderSplitter();
            RenderContent();
        }
        
        private float splitterPos;
        private Vector2 tabScrollView;
        private void RenderTab()
        {
             tabScrollView = GUILayout.BeginScrollView(tabScrollView,
                GUILayout.Width(splitterPos),
                GUILayout.MaxWidth(splitterPos),
                GUILayout.MinWidth(splitterPos));
            windowProxy.Tab();
            GUILayout.EndScrollView();
        }
        
        private readonly float splitterWidth = 5;
        private Rect splitterRect;
        private bool dragging;
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
            if (Event.current == null)
            {
                return;
            }
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

        private Vector2 contentScrollView;
        private void RenderContent()
        {
            contentScrollView = GUILayout.BeginScrollView(contentScrollView, GUILayout.ExpandWidth(true));
            windowProxy.Content();
            GUILayout.EndScrollView();
        }
    }
}
