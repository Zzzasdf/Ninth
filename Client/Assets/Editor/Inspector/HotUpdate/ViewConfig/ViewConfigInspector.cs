using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.Editor
{
    [CustomEditor(typeof(ViewConfig))]
    public class ViewConfigInspector : UnityEditor.Editor
    {
        private BaseView baseView;
        private BaseChildView baseChildView;
        public override void OnInspectorGUI()
        {
            var viewConfig = (ViewConfig)target;

            if (GUILayout.Button("一键删除"))
            {
                viewConfig.ViewInfos = new();
            }
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(viewConfig);
                AssetDatabase.SaveAssetIfDirty(viewConfig);
            }

            using (new GUILayout.VerticalScope("FrameBox"))
            {
                var newViewConfig = new List<ViewConfig.ViewInfo>();
                for (var i = 0; i < viewConfig.ViewInfos.Count; i++)
                {
                    GUILayout.Space(10);
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label($"[{i}] =>");
                        var viewInfo = viewConfig.ViewInfos[i];
                        BaseView viewModify = null;
                        using (new GUILayout.VerticalScope())
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                var view = AssetDatabase.LoadAssetAtPath<BaseView>(viewInfo.Path);
                                viewModify = (BaseView)EditorGUILayout.ObjectField(view, typeof(BaseView), false);
                                var assetPath = AssetDatabase.GetAssetPath(viewModify);
                                if (!string.IsNullOrEmpty(assetPath) && File.Exists(assetPath))
                                {
                                    var fileInfo = new FileInfo(assetPath);
                                    var path = fileInfo.FullName[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
                                    if (path != viewInfo.Path)
                                    {
                                        viewInfo.Key = viewModify.GetType().Name;
                                        viewInfo.Path = path;
                                    }
                                }
                                else
                                {
                                    viewInfo.Key = null;
                                    viewInfo.Path = null;
                                }

                                if (viewModify != null)
                                {
                                    // GUI.enabled = false;
                                    // viewInfo.Key = EditorGUILayout.TextField(viewModify.GetType().Name);
                                    // viewInfo.Path = EditorGUILayout.TextField(viewInfo.Path);
                                    // GUI.enabled = true;
                                    viewInfo.Hierarchy = (VIEW_HIERARCHY)EditorGUILayout.EnumPopup(viewInfo.Hierarchy);
                                    viewInfo.Weight = EditorGUILayout.IntField(viewInfo.Weight);
                                }

                                if (!GUILayout.Button("Remove"))
                                {
                                    newViewConfig.Add(viewInfo);
                                }
                            }

                            if (viewModify != null)
                            {
                                RenderChildView(viewInfo, viewConfig.DefaultChildWeight);
                            }
                            else
                            {
                                viewInfo.ChildViewInfos = null;
                            }
                        }
                    }
                }

                viewConfig.ViewInfos = newViewConfig;
                baseView = (BaseView)EditorGUILayout.ObjectField(baseView, typeof(BaseView), false);
                if (baseView != null)
                {
                    var assetPath = AssetDatabase.GetAssetPath(baseView);
                    var fileInfo = new FileInfo(assetPath);
                    var path = fileInfo.FullName[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
                    var viewInfo = new ViewConfig.ViewInfo
                    {
                        Key = baseView.GetType().Name,
                        Path = path,
                        Hierarchy = viewConfig.DefaultHierarchy,
                        Weight = viewConfig.DefaultWeight
                    };
                    viewConfig.ViewInfos.Add(viewInfo);
                    baseView = null;
                }

                GUILayout.Space(50);
                using (new GUILayout.HorizontalScope("FrameBox"))
                {
                    using (new GUILayout.VerticalScope("FrameBox"))
                    {
                        GUILayout.Label("未分配的 View");
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label("层级");
                            viewConfig.DefaultHierarchy = (VIEW_HIERARCHY)EditorGUILayout.EnumPopup(viewConfig.DefaultHierarchy);
                            GUILayout.Label("权重");
                            viewConfig.DefaultWeight = EditorGUILayout.IntField(viewConfig.DefaultWeight);
                        }
                    }

                    using (new GUILayout.VerticalScope("FrameBox"))
                    {
                        GUILayout.Label("未分配的 ChildView");
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label("权重");
                            viewConfig.DefaultChildWeight = EditorGUILayout.IntField(viewConfig.DefaultChildWeight);
                        }
                    }
                }
            }
        }

        private void RenderChildView(ViewConfig.ViewInfo viewInfo, int defaultChildWeight)
        {
            using (new GUILayout.VerticalScope())
            {
                List<ViewConfig.ChildViewInfo> newChildViewConfig = null;
                if (viewInfo.ChildViewInfos != null)
                {
                    for (var j = 0; j < viewInfo.ChildViewInfos.Count; j++)
                    {
                        using (new GUILayout.VerticalScope())
                        {
                            var childViewInfo = viewInfo.ChildViewInfos[j];
                            BaseChildView childViewModify = null;
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label($"[{j}] =>");
                                var childView = AssetDatabase.LoadAssetAtPath<BaseChildView>(childViewInfo.Path);
                                childViewModify = (BaseChildView)EditorGUILayout.ObjectField(childView, typeof(BaseChildView), false);
                                var childAssetPath = AssetDatabase.GetAssetPath(childViewModify);
                                if (!string.IsNullOrEmpty(childAssetPath) && File.Exists(childAssetPath))
                                {
                                    var fileInfo = new FileInfo(childAssetPath);
                                    var path = fileInfo.FullName[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
                                    if (path != childViewInfo.Path)
                                    {
                                        childViewInfo.Key = childViewModify.GetType().Name;
                                        childViewInfo.Path = path;
                                    }
                                }
                                else
                                {
                                    childViewInfo.Key = null;
                                    childViewInfo.Path = null;
                                }

                                if (childViewModify != null)
                                {
                                    // GUI.enabled = false;
                                    // childViewInfo.Key = EditorGUILayout.TextField(childViewModify.GetType().Name);
                                    // childViewInfo.Path = EditorGUILayout.TextField(childViewInfo.Path);
                                    // GUI.enabled = true;
                                    childViewInfo.Weight = EditorGUILayout.IntField(childViewInfo.Weight);
                                }

                                if (!GUILayout.Button("Remove"))
                                {
                                    newChildViewConfig ??= new();
                                    newChildViewConfig.Add(childViewInfo);
                                }
                            }
                        }
                    }
                }

                viewInfo.ChildViewInfos = newChildViewConfig?.Count == 0 ? null : newChildViewConfig;
                baseChildView = (BaseChildView)EditorGUILayout.ObjectField(baseChildView, typeof(BaseChildView), false);
                if (baseChildView != null)
                {
                    var assetPath = AssetDatabase.GetAssetPath(baseChildView);
                    var fileInfo = new FileInfo(assetPath);
                    var path = fileInfo.FullName[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
                    var childViewInfo = new ViewConfig.ChildViewInfo
                    {
                        Key = baseChildView.GetType().Name,
                        Path = path,
                        Weight = defaultChildWeight
                    };
                    viewInfo.ChildViewInfos ??= new();
                    viewInfo.ChildViewInfos.Add(childViewInfo);
                    baseChildView = null;
                }
            }
        }
    }
}