using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearchWindow
    {
        private static List<string> m_SearchObjList;
        private static int m_Count;

        private static void SetSearchObj()
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
            for (int index = m_Count; index < m_SearchObjList.Count; index++)
            {
                if (m_SearchObjList.Count >= 1)
                {
                    m_SearchObjList.RemoveAt(m_SearchObjList.Count - 1);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            for (int index = 0; index < m_SearchObjList.Count; index++)
            {
                EditorGUILayout.BeginHorizontal();
                m_SearchObjList[index] = EditorGUILayout.TextField($"Value{index + 1}", m_SearchObjList[index]);
                if (GUILayout.Button("Remove"))
                {
                    m_SearchObjList.RemoveAt(index);
                    m_Count--;
                }
                EditorGUILayout.EndHorizontal();
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
