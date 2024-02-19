using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor.Window
{
    public class WindowProxy: EditorWindow, IWindowProxy
    {
        private static WindowProxy window;
        
        [MenuItem("Tools/WindowCollect/Open")]
        private static void PanelOpen()
        {
            window = GetWindow<WindowProxy>();
            //window.position = new Rect(200, 200, 800, 500);
            window.position = new Rect(2200, 200, 1000, 700);
            window.splitterPos = 150;
        }

        [MenuItem("Tools/WindowCollect/Close")]
        private static void PanelClose()
        {
            GetWindow<WindowProxy>().Close();
            // if (window == null)
            // { 
            //     return;
            // }
            // window.Saves();
            // window.Close();
        }

        private readonly IWindowConfig windowConfig;
        private readonly VContainer.IObjectResolver resolver;
        
        [Inject]
        public WindowProxy(IWindowConfig windowConfig, VContainer.IObjectResolver resolver)
        {
            $"windowConfig Value: {windowConfig}".Log();
            this.windowConfig = windowConfig;
            this.resolver = resolver;
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
            windowConfig.CurrentTab = (Tab)GUILayout.SelectionGrid((int)(windowConfig.CurrentTab), tabs, 1);
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
            var value = windowConfig.Get(windowConfig.CurrentTab);
            if (value.HasValue)
            {
                // resolver.Resolve<AssetConfig>(value.Value.type);
            }
            GUILayout.EndScrollView();
        }

        private void Saves()
        {
            // TODO 保存 window 相关 json
        }
    }
}