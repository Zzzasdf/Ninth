// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using System;
// using System.Linq;
// using System.Collections.ObjectModel;
// using VContainer;
//
// namespace Ninth.Editor.Window
// {
//     public class Proxy :  EditorWindow
//     {
//         private static Proxy? window;
//         
//         [MenuItem("Tools/WindowCollect/Open")]
//         private static void PanelOpen()
//         {
//             window = GetWindow<Proxy>();
//             //window.position = new Rect(200, 200, 800, 500);
//             window.position = new Rect(2200, 200, 1000, 700);
//             window.splitterPos = 150;
//         }
//
//         [MenuItem("Tools/WindowCollect/Close")]
//         private static void PanelClose()
//         {
//             if (window == null)
//             {
//                 return;
//             }
//             window.Saves();
//             window.Close();
//         }
//
//         private readonly IConfig config;
//         private readonly IObjectResolver resolver;
//         
//         [Inject]
//         public Proxy(IConfig config, IObjectResolver resolver)
//         {
//             this.config = config;
//             this.resolver = resolver;
//         }
//         
//         private void OnGUI()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 RenderTags();
//                 RenderSplitter();
//                 RenderContent();
//             }
//         }
//
//         private float splitterPos;
//         private Vector2 tabScrollView;
//         private void RenderTags()
//         {
//             tabScrollView = GUILayout.BeginScrollView(tabScrollView,
//                 GUILayout.Width(splitterPos),
//                 GUILayout.MaxWidth(splitterPos),
//                 GUILayout.MinWidth(splitterPos));
//
//             var tabs = config.Keys.ToArray().ToCurrLanguage();
//             config.CurrentTab = (Tab)GUILayout.SelectionGrid((int)config.CurrentTab, tabs, 1);
//             GUILayout.EndScrollView();
//         }
//
//         private readonly float splitterWidth = 5;
//         private Rect splitterRect;
//         private bool dragging;
//         private void RenderSplitter()
//         {
//             // 分割线
//             GUILayout.Box("",
//                 GUILayout.Width(splitterWidth),
//                 GUILayout.MaxWidth(splitterWidth),
//                 GUILayout.MinWidth(splitterWidth),
//                 GUILayout.ExpandHeight(true));
//             splitterRect = GUILayoutUtility.GetLastRect();
//             // 分割线事件
//             if (Event.current == null)
//             {
//                 return;
//             }
//             switch (Event.current.rawType)
//             {
//                 case EventType.MouseDown:
//                     if (splitterRect.Contains(Event.current.mousePosition))
//                     {
//                         dragging = true;
//                     }
//                     break;
//                 case EventType.MouseDrag:
//                     if (dragging)
//                     {
//                         splitterPos += Event.current.delta.x;
//                         Repaint();
//                     }
//                     break;
//                 case EventType.MouseUp:
//                     if (dragging)
//                     {
//                         dragging = false;
//                     }
//                     break;
//             }
//         }
//
//         private Vector2 contentSrollView;
//         private void RenderContent()
//         {
//             // 页签内容
//             contentSrollView = GUILayout.BeginScrollView(contentSrollView, GUILayout.ExpandWidth(true));
//             resolver.Resolve(config.Get(config.CurrentTab));
//             GUILayout.EndScrollView();
//         }
//
//         private void Saves()
//         {
//             config.Save();
//             // TODO
//         }
//     }
// }