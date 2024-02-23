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
        [MenuItem("Tools/WindowCollect/Open")]
        private static void PanelOpen()
        {
            window = GetWindow<WindowCollect>();
            // window.position = new Rect(200, 200, 800, 500);
            window.position = new Rect(2200, 200, 1000, 700);
            window.splitterPos = 150;
        }

        [MenuItem("Tools/WindowCollect/Close")]
        private static void PanelClose()
        {
            window.Close();
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
            jsonProxy.ToJson<WindowJson, Tab>();
            jsonProxy.ToJson<BuildJson>(Tab.Build, false);
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            using var horizontalScope = new GUILayout.HorizontalScope();
            RenderTags();
            RenderSplitter();
            RenderContent();
        }
        
        private float splitterPos;
        private Vector2 tabScrollView;
        private void RenderTags()
        {
             tabScrollView = GUILayout.BeginScrollView(tabScrollView,
                GUILayout.Width(splitterPos),
                GUILayout.MaxWidth(splitterPos),
                GUILayout.MinWidth(splitterPos));

             var tabs = windowProxy.Keys().ToArrayString();
            var tab = (Tab)GUILayout.SelectionGrid((int)(windowProxy.GetEnumType<Tab>()), tabs, 1);
            windowProxy.SetEnumType<Tab>((int)tab);
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
            // 页签内容
            contentScrollView = GUILayout.BeginScrollView(contentScrollView, GUILayout.ExpandWidth(true));
            var tab = windowProxy.GetEnumType<Tab>();
            var type = windowProxy.Get(tab);
            (resolver.Resolve(type) as IStartable)?.Start();
            GUILayout.EndScrollView();
        }
    }
}
