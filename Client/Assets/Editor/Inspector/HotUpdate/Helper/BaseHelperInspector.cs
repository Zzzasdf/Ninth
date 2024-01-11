using Ninth.HotUpdate;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public abstract class BaseHelperInspector<T, TEnum, TMap> : UnityEditor.Editor
        where T : BaseHelper<TEnum, TMap>
        where TEnum : Enum
        where TMap : BaseHelperMap<TEnum, TMap>, new()
    {
        private T helper;
        private bool editLock;
        private Vector2 vec2;
        private List<string> addKeys;
        private List<UnityEngine.Object> addValues;
        private string tip;

        private void OnEnable()
        {
            helper = target as T;
            if (helper == null)
            {
                return;
            }
            if (helper.Enums == null)
            {
                helper.Enums = new List<TEnum>();
            }
            if (helper.DataBarModes == null)
            {
                helper.DataBarModes = new List<TEnum>();
            }
            if (helper.Keys == null)
            {
                helper.Keys = new List<KeyList>();
            }
            if (helper.Values == null)
            {
                helper.Values = new List<ValueList>();
            }
        }

        public override void OnInspectorGUI()
        {
            RenderEditLock();
            RenderScrollView();
            RenderValue();
            RenderTip();
        }

        private void RenderEditLock()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                SetHelperBarMode();
                string[] barNames = new string[] { "Unlock", "Lock"};
                helper.EditLock = GUILayout.Toolbar(helper.EditLock, barNames);
                editLock = helper.EditLock == 1;
            }
        }

        private void SetHelperBarMode()
        {
            if (editLock) GUI.enabled = false;
            TEnum tEnum = (TEnum)EditorGUILayout.EnumFlagsField(helper.Enum);

            if (!tEnum.Equals(helper.Enum))
            {
                helper.Enum = tEnum;

                // Check
                List<TEnum> tNewEnums = new List<TEnum>();
                List<KeyList> newKeys = new List<KeyList>();
                List<ValueList> newValues = new List<ValueList>();
                for(int index = 0; index < helper.Enums.Count; index++)
                {
                    if (helper.Keys[index].Values.Count > 0)
                    {
                        tNewEnums.Add(helper.DataBarModes[index]);
                        newKeys.Add(helper.Keys[index]);
                        newValues.Add(helper.Values[index]);
                        helper.Enum = TEnumBitAnd(helper.Enum, helper.Enums[index]);
                        tip = $"Data in use in this enumeration cannot be deleted";
                    }
                }
                helper.DataBarModes = tNewEnums;
                helper.Keys = newKeys;
                helper.Values = newValues;

                helper.Enums.Clear();
                foreach (var item in Enum.GetValues(typeof(TEnum)))
                {
                    TEnum barMode = (TEnum)item;
                    if (helper.Enum.HasFlag(barMode))
                    {
                        helper.Enums.Add(barMode);
                    }
                }
            }
            if (helper.EnumIndex > helper.Enums.Count - 1)
            {
                helper.EnumIndex = helper.Enums.Count - 1;
            }
            if (helper.Enums.Count > 0 && helper.EnumIndex < 0)
            {
                helper.EnumIndex = 0;
            }
            if (editLock) GUI.enabled = true;
        }

        protected abstract TEnum TEnumBitAnd(TEnum tEnum1, TEnum tEnum2);

        private void RenderScrollView()
        {
            vec2 = EditorGUILayout.BeginScrollView(vec2);
            List<string> barNames = new List<string>();
            for (int index = 0; index < helper.Enums.Count; index++)
            {
                TEnum barMode = helper.Enums[index];
                if (helper.DataBarModes.Count - 1 < index
                    || !helper.DataBarModes[index].Equals(helper.Enums[index]))
                {
                    helper.DataBarModes.Insert(index, barMode);
                    helper.Keys.Insert(index, new KeyList());
                    helper.Values.Insert(index, new ValueList());
                }
                barNames.Add(helper.Enums[index].ToString() + $"({helper.Keys[index].Values.Count})");
            }
            int modeIndex = GUILayout.SelectionGrid(helper.EnumIndex, barNames.ToArray(), 3);

            if (modeIndex != helper.EnumIndex)
            {
                tip = string.Empty;
                helper.EnumIndex = modeIndex;
            }
            EditorGUILayout.EndScrollView();
        }

        private void RenderValue()
        {
            if (editLock) GUI.enabled = false;
            if (helper.Enums != null
                && helper.EnumIndex < helper.Enums.Count
                && helper.Enums.Count > 0)
            {
                TEnum helperBarMode = helper.Enums[helper.EnumIndex];
                Type type = helper.Map().GetType(helperBarMode);
                SetDictionary(ref helper.Keys[helper.EnumIndex].Values, ref helper.Values[helper.EnumIndex].Values, type);
            }
            if (editLock) GUI.enabled = true;
        }

        private void SetDictionary(ref List<string> names, ref List<UnityEngine.Object> objs, Type type)
        {
            // Items
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
            if (names.Count != 0 && names.Count == objs.Count)
            {
                using(new EditorGUILayout.HorizontalScope())
                {
                    bool isHaveMissing = false;
                    for (int index = 0; index < objs.Count; index++)
                    {
                        if (objs[index] == null)
                        {
                            isHaveMissing = true;
                            break;
                        }
                    }
                    if (isHaveMissing)
                    {
                        if (GUILayout.Button("ClearMissing"))
                        {
                            List<string> newNames = new List<string>();
                            List<UnityEngine.Object> newObjs = new List<UnityEngine.Object>();
                            for(int index = 0; index < objs.Count; index++)
                            {
                                if (objs[index] != null)
                                {
                                    newNames.Add(names[index]);
                                    newObjs.Add(objs[index]);
                                }
                            }
                            names = newNames;
                            objs = newObjs;
                        }
                    }
                }

                List<string> newNameList = null;
                List<UnityEngine.Object> newObjList = null;
                bool isRemove = false;
                for (int index = 0; index < names.Count; index++)
                {
                    using(new EditorGUILayout.HorizontalScope())
                    {
                        string name = EditorGUILayout.TextField(names[index]);
                        if (!string.IsNullOrEmpty(name) && !names.Contains(name))
                        {
                            names[index] = name;
                        }
                        UnityEngine.Object obj = EditorGUILayout.ObjectField(objs[index], type, true);
                        if (obj != null && !objs.Contains(obj))
                        {
                            objs[index] = obj;
                        }
                        if (GUILayout.Button("Remove"))
                        {
                            newNameList = new List<string>();
                            newObjList = new List<UnityEngine.Object>();
                            for(int i = 0; i < names.Count; i++)
                            {
                                if (i == index)
                                {
                                    continue;
                                }
                                newNameList.Add(names[i]);
                                newObjList.Add(objs[i]);
                            }
                            isRemove = true;
                        }
                    }
                }
                if(isRemove)
                {
                    names = newNameList;
                    objs = newObjList;
                }
            }

            // AddArea
            if (!editLock)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("AddItem", EditorStyles.boldLabel);
                using(new EditorGUILayout.HorizontalScope())
                {
                    // Add
                    int modeIndex = helper.EnumIndex;
                    if (addKeys == null)
                    {
                        addKeys = new List<string>();
                    }
                    int count = addKeys.Count;
                    for (int index = count; index <= modeIndex; index++)
                    {
                        addKeys.Add(null);
                    }
                    addKeys[modeIndex] = EditorGUILayout.TextField(addKeys[modeIndex]);

                    if (addValues == null)
                    {
                        addValues = new List<UnityEngine.Object>();
                    }
                    count = addValues.Count;
                    for (int index = count; index <= modeIndex; index++)
                    {
                        addValues.Add(null);
                    }
                    addValues[modeIndex] = EditorGUILayout.ObjectField(addValues[modeIndex], type, true);

                    // Check
                    if (addValues[modeIndex] != null)
                    {
                        var checkValue = addValues[modeIndex];
                        if (objs.Contains(checkValue))
                        {
                            addValues[modeIndex] = null;
                            int keyIndex = objs.IndexOf(checkValue);
                            tip = $"This value already exists, key is {names[keyIndex]}";
                        }
                    }
                    if (addValues[modeIndex] != null && string.IsNullOrEmpty(addKeys[modeIndex]))
                    {
                        addKeys[modeIndex] = addValues[modeIndex].name;
                    }

                    // AddFunc
                    if (GUILayout.Button("Confirm"))
                    {
                        if (addValues[modeIndex] == null)
                        {
                            tip = "Value cannot be empty!!";
                        }
                        else if (string.IsNullOrEmpty(addKeys[modeIndex]))
                        {
                            tip = "Key cannot be empty!!";
                        }
                        else if (names.Contains(addKeys[modeIndex]))
                        {
                            tip = "This key already exists!!";
                        }
                        else
                        {
                            names.Add(addKeys[modeIndex]);
                            objs.Add(addValues[modeIndex]);
                            addKeys[modeIndex] = null;
                            addValues[modeIndex] = null;
                            tip = null;
                        }
                    }
                }
            }
        }

        private void RenderTip()
        {
            if (helper.EditLock == 1)
            {
                GUI.enabled = false;
            }
            if (!string.IsNullOrEmpty(tip))
            {
                EditorGUILayout.LabelField(tip, EditorStyles.boldLabel);
            }
            if (helper.EditLock == 1)
            {
                GUI.enabled = true;
            }
        }
    }
}