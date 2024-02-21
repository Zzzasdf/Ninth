using System;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor.Window
{
    public class WindowProxy: EditorWindow, IWindowProxy
    {
        [MenuItem("Tools/WindowCollect/Open")]
        private static void PanelOpen()
        {
            var window = GetWindow<WindowProxy>();
            // window.position = new Rect(200, 200, 800, 500);
            window.position = new Rect(2200, 200, 1000, 700);
            window.splitterPos = 150;
        }

        [MenuItem("Tools/WindowCollect/Close")]
        private static void PanelClose()
        {
            GetWindow<WindowProxy>().Close();
        }

        public static void SubscribeResolver(IObjectResolver resolver)
        {
            WindowProxy.resolver = resolver;
        }
        private static IObjectResolver resolver;
        private IWindowConfig windowConfig;
        private IJsonProxy jsonProxy;

        private void OnEnable()
        {
            windowConfig = resolver.Resolve<IWindowConfig>();
            jsonProxy = resolver.Resolve<IJsonProxy>();
        }

        private void OnDisable()
        {
            // 保存 window 相关 json 
            jsonProxy.ToJson<IWindowConfig, Tab>(windowConfig);
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

            var tabs = windowConfig.Keys().ToArrayString();
            var tab = (Tab)GUILayout.SelectionGrid((int)(windowConfig.GetEnumType<Tab>()), tabs, 1);
            windowConfig.SetEnumType<Tab>((int)tab);
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
            var tab = windowConfig.GetEnumType<Tab>();
            var type = windowConfig.Get(tab);
            (resolver.Resolve(type) as IStartable)?.Start();
            GUILayout.EndScrollView();
        }
    }
}