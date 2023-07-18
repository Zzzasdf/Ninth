using UnityEngine;
using UnityEditor;
using System.Diagnostics;

namespace Ninth.Editor
{
    public class OtherSettings<T>
        where T : IOtherSettingsData, new()
    {
        private T data;

        public OtherSettings()
        {
            data = new T();
        }

        public void OnGUI()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("AllFoldout"))
                {
                    data.AllFoldout = false;
                }
                if (GUILayout.Button("AllUnFoldout"))
                {
                    data.AllFoldout = true;
                }
            }

            using(new EditorGUILayout.VerticalScope("Box"))
            {
                data.AppFoldout = EditorGUILayout.Foldout(data.AppFoldout, "App");
                if (data.AppFoldout)
                {
                    foreach (var item in data.AppOperationDic)
                    {
                        using(new EditorGUILayout.HorizontalScope("frameBox"))
                        {
                            EditorGUILayout.LabelField(item.Key);
                            foreach (var item2 in item.Value)
                            {
                                if (GUILayout.Button(item2.Key))
                                {
                                    Process.Start(item2.Value);
                                }
                            }
                        }
                    }
                }
            }
            
            using(new EditorGUILayout.VerticalScope("Box"))
            {
                data.BrowserFoldout = EditorGUILayout.Foldout(data.BrowserFoldout, "Browser");
                if (data.BrowserFoldout)
                {
                    foreach (var item in data.BrowserOperationDic)
                    {
                        using (new EditorGUILayout.HorizontalScope("frameBox"))
                        {
                            EditorGUILayout.LabelField(item.Key);
                            foreach (var item2 in item.Value)
                            {
                                if (GUILayout.Button(item2.Key))
                                {
                                    Process.Start(item2.Value);
                                }
                            }
                        }
                    }
                }
            }

            using(new EditorGUILayout.VerticalScope("Box"))
            {
                data.DirectoryFoldout = EditorGUILayout.Foldout(data.DirectoryFoldout, "Directory");
                if (data.DirectoryFoldout)
                {
                    foreach (var item in data.DirectoryOperationDic)
                    {
                        using (new EditorGUILayout.HorizontalScope("frameBox"))
                        {
                            EditorGUILayout.LabelField(item.Key);
                            foreach (var item2 in item.Value)
                            {
                                if (GUILayout.Button(item2.Key))
                                {
                                    item2.Value?.Invoke();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
