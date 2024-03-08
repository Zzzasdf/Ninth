using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class EditorWindowUtility
    {
        public static void ToolBar(ReactiveProperty<int> selected, string[] texts)
        {
            var temp = GUILayout.Toolbar(selected.Value, texts);
            if (temp == selected.Value) return;
            selected.Value = temp;
        }
        
        public static void SelectionGrid(ReactiveProperty<int> selected, string[] texts, int xCount)
        {
            var temp = GUILayout.SelectionGrid(selected.Value, texts, xCount);
            if (temp == selected.Value) return; 
            selected.Value = temp;
        }
        
        public static void SelectFolder(string label, ReactiveProperty<string> folder, string defaultName, Func<string, bool>? condition = null, bool isModify = true)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.TextField(label, folder.Value);
                GUI.enabled = true;
                if (isModify && GUILayout.Button("浏览"))
                {
                    var temp = EditorUtility.OpenFolderPanel("选择目标文件夹", folder.Value, defaultName);
                    if (string.IsNullOrEmpty(temp))
                    {
                        return;
                    }
                    if (temp.Equals(folder.Value))
                    {
                        return;
                    }
                    if (condition != null && !condition.Invoke(temp))
                    {
                        return;
                    }
                    folder.Value = temp;
                }
            }
        }
        
        public static void SelectFolderCollect(string label, ReactiveProperty<List<string>> folders, string defaultName, Func<string, bool>? condition = null)
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField(label);
                Stack<int>? removes = null;
                for (var i = 0; i < folders.Value.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUI.enabled = false;
                        EditorGUILayout.TextField($"[{i}] =>", folders.Value[i]);
                        GUI.enabled = true;
                        if (GUILayout.Button("浏览"))
                        {
                            var temp = EditorUtility.OpenFolderPanel("选择目标文件夹", folders.Value[i], defaultName);
                            if (string.IsNullOrEmpty(temp))
                            {
                                return;
                            }

                            if (temp.Equals(folders.Value[i]))
                            {
                                return;
                            }

                            if (condition != null && !condition.Invoke(temp))
                            {
                                return;
                            }

                            folders.Value[i] = temp;
                        }

                        if (GUILayout.Button("移除"))
                        {
                            removes ??= new Stack<int>();
                            removes.Push(i);
                        }
                    }
                }
                if (GUILayout.Button("++++++新增资源组++++++"))
                {
                    folders.Value.Add(string.Empty);
                }
                while (removes is { Count: > 0 })
                {
                    var index = removes.Pop();
                    folders.Value.RemoveAt(index);
                }
            }
        }
        
        public static void IntPopup<T>(string label, ReactiveProperty<T> selectedValue, string[] displayedOptions, int[] optionValues, bool isModify = true)
            where T: struct
        {
            if (!isModify)
            {
                GUI.enabled = false;
            }
            selectedValue.Value = (T)(object)EditorGUILayout.IntPopup(label, (int)(object)selectedValue.Value, displayedOptions, optionValues);
            if (!isModify)
            {
                GUI.enabled = true;
            }
        }
    }
}