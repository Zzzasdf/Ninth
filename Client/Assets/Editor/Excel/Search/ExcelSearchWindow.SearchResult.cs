using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearchWindow
    {
        private static Dictionary<string, Dictionary<string, LinkedList<SearchResultCell>>> m_SearchResultDic;
        private static Dictionary<string, bool> m_FoldOutDic;
        private static Dictionary<string, Dictionary<string, bool>> m_FoldOutDic2;

        private static Vector2 m_SearchResultSV_V2;

        private static void ExchangeSearchResultMode()
        {
            string btnName = (SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode switch
            {
                SearchResultMode.Table => "TableMode",
                SearchResultMode.Value => "ValueMode",
                _ => throw new System.NotImplementedException(),
            };
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(btnName, EditorStyles.miniButtonMid, GUILayout.Width(85)))
            {
                switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
                {
                    case SearchResultMode.Table:
                        {
                            PlayerPrefsDefine.ExcelSearchResultShowMode = (int)SearchResultMode.Value;
                            m_SearchResultInit = false;
                            break;
                        }
                    case SearchResultMode.Value:
                        {
                            PlayerPrefsDefine.ExcelSearchResultShowMode = (int)SearchResultMode.Table;
                            m_SearchResultInit = false;
                            break;
                        }
                }
            }
            GUILayout.EndHorizontal();
        }

        private static void SetSearchResult()
        {
            if (!m_SearchResultInit)
            {
                m_SearchResultInit = !m_SearchResultInit;
                if (m_SearchResultDic == null)
                {
                    m_SearchResultDic = new Dictionary<string, Dictionary<string, LinkedList<SearchResultCell>>>();
                }
                else
                {
                    m_SearchResultDic.Clear();
                }
                switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
                {
                    case SearchResultMode.Table:
                        {
                            for (int index = 0; index < m_SearchResult.Count; index++)
                            {
                                string key = m_SearchResult[index].Path;
                                string key2 = m_SearchResult[index].SearchValue;
                                if (!m_SearchResultDic.ContainsKey(key))
                                {
                                    m_SearchResultDic.Add(key, new Dictionary<string, LinkedList<SearchResultCell>>());
                                }
                                if (!m_SearchResultDic[key].ContainsKey(key2))
                                {
                                    m_SearchResultDic[key].Add(key2, new LinkedList<SearchResultCell>());
                                }
                                m_SearchResultDic[key][key2].AddLast(m_SearchResult[index]);
                            }
                            break;
                        }
                    case SearchResultMode.Value:
                        {
                            for (int index = 0; index < m_SearchResult.Count; index++)
                            {
                                string key = m_SearchResult[index].SearchValue;
                                string key2 = m_SearchResult[index].Path;
                                if (!m_SearchResultDic.ContainsKey(key))
                                {
                                    m_SearchResultDic.Add(key, new Dictionary<string, LinkedList<SearchResultCell>>());
                                }
                                if (!m_SearchResultDic[key].ContainsKey(key2))
                                {
                                    m_SearchResultDic[key].Add(key2, new LinkedList<SearchResultCell>());
                                }
                                m_SearchResultDic[key][key2].AddLast(m_SearchResult[index]);
                            }
                            break;
                        }
                }
                if (m_FoldOutDic == null)
                {
                    m_FoldOutDic = new Dictionary<string, bool>();
                }
                else
                {
                    m_FoldOutDic.Clear();
                }
                if (m_FoldOutDic2 == null)
                {
                    m_FoldOutDic2 = new Dictionary<string, Dictionary<string, bool>>();
                }
                else
                {
                    m_FoldOutDic2.Clear();
                }
                foreach (var item in m_SearchResultDic)
                {
                    m_FoldOutDic.Add(item.Key, true);
                    m_FoldOutDic2.Add(item.Key, new Dictionary<string, bool>());
                    foreach (var item2 in item.Value)
                    {
                        m_FoldOutDic2[item.Key].Add(item2.Key, true);
                    }
                }
            }

            m_SearchResultSV_V2 = EditorGUILayout.BeginScrollView(m_SearchResultSV_V2);
            switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
            {
                case SearchResultMode.Table:
                    {
                        SetSearchResultTableMode();
                        break;
                    }
                case SearchResultMode.Value:
                    {
                        SetSearchResultValueMode();
                        break;
                    }
            }
            EditorGUILayout.EndScrollView();
        }

        private static void SetSearchResultTableMode()
        {
            EditorGUILayout.BeginVertical();
            foreach (var item in m_SearchResultDic)
            {
                string key = item.Key;
                var value = item.Value;
                int count = 0;
                foreach (var item2 in value)
                {
                    count += item2.Value.Count;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{key}({count})", EditorStyles.boldLabel);
                if (GUILayout.Button("Enter"))
                {
                    Process.Start(key);
                }
                if (GUILayout.Button(m_FoldOutDic[key] ? $"Foldout({count})" : $"Unfold({count})"))
                {
                    m_FoldOutDic[key] = !m_FoldOutDic[key];
                }
                EditorGUILayout.EndHorizontal();

                if (!m_FoldOutDic[key])
                {
                    EditorGUI.indentLevel++;
                    for (int index = 0; index < m_SearchObjResultList.Count; index++)
                    {
                        if (value.ContainsKey(m_SearchObjResultList[index]))
                        {
                            string key2 = m_SearchObjResultList[index];
                            var value2 = value[key2];

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Value:[{key2}]", EditorStyles.boldLabel);
                            if (GUILayout.Button(m_FoldOutDic2[key][key2] ? $"Foldout({value2.Count})" : $"Unfold({value2.Count})"))
                            {
                                m_FoldOutDic2[key][key2] = !m_FoldOutDic2[key][key2];
                            }
                            EditorGUILayout.EndHorizontal();

                            if (!m_FoldOutDic2[key][key2])
                            {
                                EditorGUI.indentLevel++;
                                foreach (var one in value2)
                                {
                                    EditorGUILayout.TextArea($"第{one.SheetIndex}页，第{one.Row}行，第{one.Col}列\nCellValue:--[{one.CellValue}]--");
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        private static void SetSearchResultValueMode()
        {
            EditorGUILayout.BeginVertical();
            for (int index = 0; index < m_SearchObjResultList.Count; index++)
            {
                string key = m_SearchObjResultList[index];
                if (m_SearchResultDic.ContainsKey(key))
                {
                    var value = m_SearchResultDic[key];
                    int count = 0;
                    foreach (var item2 in value)
                    {
                        count += item2.Value.Count;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Value:[{key}]", EditorStyles.boldLabel);
                    if (GUILayout.Button(m_FoldOutDic[key] ? $"Foldout({count})" : $"Unfold({count})"))
                    {
                        m_FoldOutDic[key] = !m_FoldOutDic[key];
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!m_FoldOutDic[key])
                    {
                        EditorGUI.indentLevel++;
                        foreach (var item2 in value)
                        {
                            string key2 = item2.Key;
                            var value2 = item2.Value;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"{key2}({item2.Value.Count})", EditorStyles.boldLabel);
                            if (GUILayout.Button("Enter"))
                            {
                                Process.Start(key2);
                            }
                            if (GUILayout.Button(m_FoldOutDic2[key][key2] ? $"Foldout({item2.Value.Count})" : $"Unfold({item2.Value.Count})"))
                            {
                                m_FoldOutDic2[key][key2] = !m_FoldOutDic2[key][key2];
                            }
                            EditorGUILayout.EndHorizontal();

                            if (!m_FoldOutDic2[key][key2])
                            {
                                EditorGUI.indentLevel++;
                                foreach (var one in value2)
                                {
                                    EditorGUILayout.TextArea($"第{one.SheetIndex}页，第{one.Row}行，第{one.Col}列\nCellValue: --[{one.CellValue}]--");
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
