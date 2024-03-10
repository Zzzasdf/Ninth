using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class EditorWindowUtility
    {
        public static void Toolbar(ReactiveProperty<int> selected, string[] texts)
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
        
        public static bool SelectFolder(string label, ReactiveProperty<string> folder, string defaultName, Func<string, string?>? condition = null, bool isModify = true)
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.TextField(label, folder.Value);
                    GUI.enabled = true;
                    if (isModify && GUILayout.Button("浏览"))
                    {
                        folder.Value = EditorUtility.OpenFolderPanel("选择目标文件夹", folder.Value, defaultName);
                    }
                }
                using(new GUILayout.HorizontalScope())
                {
                    if (string.IsNullOrEmpty(folder.Value))
                    {
                        EditorGUILayout.HelpBox("不能为空", MessageType.Error);
                        return false;
                    }
                    var conditionHelpBox = condition?.Invoke(folder.Value);
                    if (!string.IsNullOrEmpty(conditionHelpBox))
                    {
                        EditorGUILayout.HelpBox(conditionHelpBox, MessageType.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        
        public static bool SelectFolderCollect(string label, ReactiveProperty<List<string>> folders, string defaultName, Func<string, string?>? condition = null)
        {
            var result = true;
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(label);
                    if (GUILayout.Button("++++++新增资源组++++++"))
                    {
                        folders.Value.Add(string.Empty);
                    }
                }
                Stack<int>? removes = null;
                for (var i = 0; i < folders.Value.Count; i++)
                {
                    using (new GUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUI.enabled = false;
                            EditorGUILayout.TextField($"[{i}] =>", folders.Value[i]);
                            GUI.enabled = true;
                            if (GUILayout.Button("浏览"))
                            {
                                folders.Value[i] = EditorUtility.OpenFolderPanel("选择目标文件夹", folders.Value[i], defaultName);
                            }
                            if (GUILayout.Button("移除"))
                            {
                                removes ??= new Stack<int>();
                                removes.Push(i);
                            }
                        }
                        if (string.IsNullOrEmpty(folders.Value[i]))
                        {
                            EditorGUILayout.HelpBox("不能为空", MessageType.Error);
                            result = false;
                        }
                        else
                        {
                            var conditionHelpBox = condition?.Invoke(folders.Value[i]);
                            if (!string.IsNullOrEmpty(conditionHelpBox))
                            {
                                EditorGUILayout.HelpBox(conditionHelpBox, MessageType.Error);
                                result = false;
                            }
                        }
                    }
                }
                while (removes is { Count: > 0 })
                {
                    var index = removes.Pop();
                    folders.Value.RemoveAt(index);
                }
            }
            return result;
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

        public static void TextField(string label, ReactiveProperty<string> text)
        {
            text.Value = EditorGUILayout.TextField(label, text.Value);
        }
    }
}