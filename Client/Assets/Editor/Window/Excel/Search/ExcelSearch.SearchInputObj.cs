using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        private static List<string> m_SearchObjList;
        private static int m_Count;

        private static void SetSearchInputObj()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search Obj", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            m_Count = EditorGUILayout.IntField("Count", Mathf.Max(1, m_Count));

            if(m_SearchObjList == null)
            {
                m_SearchObjList = new List<string>();
            }
            for (int index = m_SearchObjList.Count; index < m_Count; index++)
            {
                m_SearchObjList.Add(string.Empty);
            }
            List<string> newSearchObjs = new List<string>();
            for (int index = 0; index < m_Count; index++)
            {
                newSearchObjs.Add(m_SearchObjList[index]);
            }
            m_SearchObjList = newSearchObjs;

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            List<string> newSearchObjList = null;
            int newCount = 0;
            bool isRemove = false;
            for (int index = 0; index < m_SearchObjList.Count; index++)
            {
                EditorGUILayout.BeginHorizontal();
                string searchObj = EditorGUILayout.TextField($"Value{index + 1}", m_SearchObjList[index]);
                if (searchObj != m_SearchObjList[index] && !m_SearchObjList.Contains(searchObj))
                {
                    m_SearchObjList[index] = searchObj;
                }
                if (GUILayout.Button("Remove"))
                {
                    newSearchObjList = new List<string>();
                    for (int i = 0; i < m_SearchObjList.Count; i++)
                    {
                        if (i == index)
                        {
                            continue;
                        }
                        newSearchObjList.Add(m_SearchObjList[i]);
                        newCount++;
                    }
                    isRemove = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            if(isRemove)
            {
                m_SearchObjList = newSearchObjList;
                m_Count = newCount;
            }
            if (GUILayout.Button("Add"))
            {
                m_Count++;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private static List<string> GetValidSearchObj()
        {
            List<string> NonEmptySearchObjectList = new List<string>();
            for (int index = 0; index < m_SearchObjList.Count; index++)
            {
                if (!string.IsNullOrEmpty(m_SearchObjList[index]))
                {
                    NonEmptySearchObjectList.Add(m_SearchObjList[index]);
                }
            }
            return NonEmptySearchObjectList;
        }
    }
}
