using Ninth.HotUpdate;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Ninth.Editor
{
    public abstract class IHelperEditor<T, TEnum, TMap> : UnityEditor.Editor
        where T : IHelper<TEnum, TMap>
        where TEnum : Enum
        where TMap : IHelperMap<TEnum, TMap>, new()
    {
        private T m_Helper;

        private bool m_Lock;

        private Vector2 m_SV2;

        private List<string> m_AddKeys;
        private List<UnityEngine.Object> m_AddValues;
        private string m_Tip;

        private void OnEnable()
        {
            m_Helper = target as T;

            if (m_Helper == null)
            {
                return;
            }
            if (m_Helper.m_TEnums == null)
            {
                m_Helper.m_TEnums = new List<TEnum>();
            }
            if (m_Helper.m_DataBarModes == null)
            {
                m_Helper.m_DataBarModes = new List<TEnum>();
            }
            if (m_Helper.m_Keys == null)
            {
                m_Helper.m_Keys = new List<KeyList>();
            }
            if (m_Helper.m_Values == null)
            {
                m_Helper.m_Values = new List<ValueList>();
            }
        }

        

        public override void OnInspectorGUI()
        {
            SetLock();
            SetScrollView();
            SetValue();
            SetTip();
        }

        private void SetLock()
        {
            EditorGUILayout.BeginHorizontal();
            SetHelperBarMode();
            string[] barNames = new string[]
            {
                "Unlock",
                "Lock",
            };
            m_Helper.m_Lock = GUILayout.Toolbar(m_Helper.m_Lock, barNames);
            m_Lock = m_Helper.m_Lock == 1;
            EditorGUILayout.EndHorizontal();
        }

        private void SetHelperBarMode()
        {
            if (m_Lock) GUI.enabled = false;
            TEnum tEnum = (TEnum)EditorGUILayout.EnumFlagsField(m_Helper.m_TEnum);

            if (!tEnum.Equals(m_Helper.m_TEnum))
            {
                m_Helper.m_TEnum = tEnum;
                // Check
                for (int index = m_Helper.m_TEnums.Count - 1; index >= 0; index--)
                {
                    if (m_Helper.m_Keys[index].Values.Count > 0)
                    {
                        m_Helper.m_TEnum = TEnumBitAnd(m_Helper.m_TEnum, m_Helper.m_TEnums[index]);
                        m_Tip = $"Data in use in this enumeration cannot be deleted";
                    }
                    else
                    {
                        m_Helper.m_DataBarModes.RemoveAt(index);
                        m_Helper.m_Keys.RemoveAt(index);
                        m_Helper.m_Values.RemoveAt(index);
                    }
                }
                m_Helper.m_TEnums.Clear();
                foreach (var item in Enum.GetValues(typeof(TEnum)))
                {
                    TEnum barMode = (TEnum)item;
                    if (m_Helper.m_TEnum.HasFlag(barMode))
                    {
                        m_Helper.m_TEnums.Add(barMode);
                    }
                }
            }
            if (m_Helper.m_TEnumIndex > m_Helper.m_TEnums.Count - 1)
            {
                m_Helper.m_TEnumIndex = m_Helper.m_TEnums.Count - 1;
            }
            if (m_Helper.m_TEnums.Count > 0 && m_Helper.m_TEnumIndex < 0)
            {
                m_Helper.m_TEnumIndex = 0;
            }
            if (m_Lock) GUI.enabled = true;
        }

        protected abstract TEnum TEnumBitAnd(TEnum tEnum1, TEnum tEnum2);

        private void SetScrollView()
        {
            m_SV2 = EditorGUILayout.BeginScrollView(m_SV2);

            List<string> barNames = new List<string>();
            for (int index = 0; index < m_Helper.m_TEnums.Count; index++)
            {
                TEnum barMode = m_Helper.m_TEnums[index];
                if (m_Helper.m_DataBarModes.Count - 1 < index
                    || !m_Helper.m_DataBarModes[index].Equals(m_Helper.m_TEnums[index]))
                {
                    m_Helper.m_DataBarModes.Insert(index, barMode);
                    m_Helper.m_Keys.Insert(index, new KeyList());
                    m_Helper.m_Values.Insert(index, new ValueList());
                }
                barNames.Add(m_Helper.m_TEnums[index].ToString() + $"({m_Helper.m_Keys[index].Values.Count})");
            }
            int modeIndex = GUILayout.Toolbar(m_Helper.m_TEnumIndex, barNames.ToArray());

            if (modeIndex != m_Helper.m_TEnumIndex)
            {
                m_Tip = string.Empty;
                m_Helper.m_TEnumIndex = modeIndex;
            }
            EditorGUILayout.EndScrollView();
        }

        private void SetValue()
        {
            if (m_Lock) GUI.enabled = false;
            if (m_Helper.m_TEnums != null
                && m_Helper.m_TEnumIndex < m_Helper.m_TEnums.Count
                && m_Helper.m_TEnums.Count > 0)
            {
                TEnum helperBarMode = m_Helper.m_TEnums[m_Helper.m_TEnumIndex];
                Type type = m_Helper.Map().GetType(helperBarMode);
                SetDictionary(m_Helper.m_Keys[m_Helper.m_TEnumIndex].Values, m_Helper.m_Values[m_Helper.m_TEnumIndex].Values, type);
            }
            if (m_Lock) GUI.enabled = true;
        }

        private void SetDictionary(List<string> names, List<UnityEngine.Object> objs, Type type)
        {
            // Items
            if (names.Count != 0 && names.Count == objs.Count)
            {
                EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
                for (int index = names.Count - 1; index >= 0; index--)
                {
                    EditorGUILayout.BeginHorizontal();
                    string name = EditorGUILayout.TextField(names[index]);
                    if (!names.Contains(name))
                    {
                        names[index] = name;
                    }
                    UnityEngine.Object obj = EditorGUILayout.ObjectField(objs[index], type, true);
                    if (!objs.Contains(obj))
                    {
                        objs[index] = obj;
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        names.RemoveAt(index);
                        objs.RemoveAt(index);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            // AddArea
            GUILayout.Space(20);
            EditorGUILayout.LabelField("AddItem", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            // Add
            int modeIndex = m_Helper.m_TEnumIndex;
            if (m_AddKeys == null)
            {
                m_AddKeys = new List<string>();
            }
            int count = m_AddKeys.Count;
            for (int index = count; index <= modeIndex; index++)
            {
                m_AddKeys.Add(null);
            }
            m_AddKeys[modeIndex] = EditorGUILayout.TextField(m_AddKeys[modeIndex]);

            if (m_AddValues == null)
            {
                m_AddValues = new List<UnityEngine.Object>();
            }
            count = m_AddValues.Count;
            for (int index = count; index <= modeIndex; index++)
            {
                m_AddValues.Add(null);
            }
            m_AddValues[modeIndex] = EditorGUILayout.ObjectField(m_AddValues[modeIndex], type, true);

            // Check
            if (m_AddValues[modeIndex] != null)
            {
                var checkValue = m_AddValues[modeIndex];
                if (objs.Contains(checkValue))
                {
                    m_AddValues[modeIndex] = null;
                    int keyIndex = objs.IndexOf(checkValue);
                    m_Tip = $"This value already exists, key is {names[keyIndex]}";
                }
            }
            if (m_AddValues[modeIndex] != null && string.IsNullOrEmpty(m_AddKeys[modeIndex]))
            {
                m_AddKeys[modeIndex] = m_AddValues[modeIndex].name;
            }

            // AddFunc
            if (GUILayout.Button("Confirm"))
            {
                if (m_AddValues[modeIndex] == null)
                {
                    m_Tip = "Value cannot be empty!!";
                }
                else if (string.IsNullOrEmpty(m_AddKeys[modeIndex]))
                {
                    m_Tip = "Key cannot be empty!!";
                }
                else if (names.Contains(m_AddKeys[modeIndex]))
                {
                    m_Tip = "This key already exists!!";
                }
                else
                {
                    names.Add(m_AddKeys[modeIndex]);
                    objs.Add(m_AddValues[modeIndex]);
                    m_AddKeys[modeIndex] = null;
                    m_AddValues[modeIndex] = null;
                    m_Tip = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetTip()
        {
            if (m_Helper.m_Lock == 1)
            {
                GUI.enabled = false;
            }
            if (!string.IsNullOrEmpty(m_Tip))
            {
                EditorGUILayout.LabelField(m_Tip, EditorStyles.boldLabel);
            }
            if (m_Helper.m_Lock == 1)
            {
                GUI.enabled = true;
            }
        }
    }
}