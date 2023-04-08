using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ScanPrefabComponentSettings<TEnum, T>
    {
        private bool bLog;
        private Vector2 sV;

        private void SetResult()
        {
            List<string> barMenu = new List<string>();
            foreach (var item in cache)
            {
                barMenu.Add(item.Key.ToString() + $"({item.Value.Values.Count})");
            }
            mode = IntToEnum(GUILayout.Toolbar(EnumToInt(mode), barMenu.ToArray()));
            if (cache.TryGetValue(mode, out Dictionary<string, ScanInfo<T>> scanInfoDic))
            {
                List<ScanInfo<T>> scanInfos = scanInfoDic.Values.ToList();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("һ��չ��"))
                {
                    scanInfos.ForEach(x =>
                    {
                        x.IsFoldout = false;
                        x.Values.ToList().ForEach(y =>
                            y.IsComponentObjsFoldout = false);
                    });
                }
                if (GUILayout.Button("һ���۵�"))
                {
                    scanInfos.ForEach(x =>
                    {
                        x.IsFoldout = true;
                        x.Values.ToList().ForEach(y =>
                            y.IsComponentObjsFoldout = true);
                    });
                }
                string bStatus = bLog ? "��" : "��";
                if (GUILayout.Button($"��־: {bStatus}"))
                {
                    bLog = !bLog;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                Dictionary<string, List<ScanInfoItem<T>>> scanInfoItemDic = new Dictionary<string, List<ScanInfoItem<T>>>();
                scanInfos.ForEach(x =>
                {
                    x.Values.ToList().ForEach(y =>
                    {
                        if (!scanInfoItemDic.ContainsKey(y.ScanLog))
                        {
                            scanInfoItemDic.Add(y.ScanLog, new List<ScanInfoItem<T>>());
                        }
                        scanInfoItemDic[y.ScanLog].Add(y);
                    });
                });
                foreach (var item in scanInfoItemDic)
                {
                    string scanLog = item.Key;
                    List<ScanInfoItem<T>> scanInfoItems = item.Value;
                    if (scanInfoItems.Where(x => x.Handle != null).ToList().Count != 0)
                    {
                        if (GUILayout.Button($"�����޸�({scanLog})"))
                        {
                            ForceRepair(scanInfoItems);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (bLog)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int index = 0; index < scanInfos.Count; index++)
                    {
                        ScanInfo<T> scanInfo = scanInfos[index];
                        sb.Append($"[{index}] => ·����").Append(scanInfo.PrefabPath)
                                .Append(" => Ԥ���壺").Append(scanInfo.PrefabName)
                                .AppendLine();
                        sb.Append("{")
                            .AppendLine();
                        List<ScanInfoItem<T>> scanInfoItems = scanInfo.Values.ToList();
                        for (int i = 0; i < scanInfoItems.Count; i++)
                        {
                            ScanInfoItem<T> scanInfoItem = scanInfoItems[i];
                            sb.Append($"    [{scanInfoItem.ScanLog}] => ")
                                .AppendLine();
                            for (int j = 0; j < scanInfoItem.ComponentObjsPath.Count; j++)
                            {
                                sb.Append("    �������").Append(scanInfoItem.ComponentObjsName[j])
                                    .Append("    ���·����").Append(scanInfoItem.ComponentObjsPath[j])
                                    .AppendLine();
                            }
                            if (i != scanInfoItems.Count - 1)
                            {
                                sb.AppendLine();
                            }
                        }
                        sb.Append("}");
                        if (index != scanInfos.Count - 1)
                        {
                            sb.AppendLine().AppendLine();
                        }
                    }
                    EditorGUILayout.TextArea(sb.ToString());
                }

                sV = GUILayout.BeginScrollView(sV);
                for (int index = 0; index < scanInfos.Count; index++)
                {
                    ScanInfo<T> scanInfo = scanInfos[index];
                    List<ScanInfoItem<T>> scanInfoItems = scanInfo.Values.ToList();

                    bool isFoldout = scanInfo.IsFoldout;
                    string prefabPath = scanInfo.PrefabPath;
                    string prefabName = scanInfo.PrefabName;
                    GUILayout.Label($"[{index}] => ========================================================================================================");
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.TextField("Ԥ����:", prefabName);

                    int count = 0;
                    scanInfoItems.Select(x => x.ComponentObjsPath.Count).ToList().ForEach(x => count += x);
                    if (GUILayout.Button(isFoldout ? $"Foldout({count})" : $"Unfold({count})"))
                    {
                        scanInfo.IsFoldout = !scanInfo.IsFoldout;
                    }
                    if (GUILayout.Button("����"))
                    {
                        InstantiateInHierarchy(prefabPath, prefabName);
                    }
                    GUILayout.EndHorizontal();

                    if (!isFoldout)
                    {
                        EditorGUILayout.TextField("·��:", prefabPath);
                        for (int i = 0; i < scanInfoItems.Count; i++)
                        {
                            ScanInfoItem<T> scanInfoItem = scanInfoItems[i];
                            bool isComponentObjsFoldout = scanInfoItem.IsComponentObjsFoldout;
                            List<string> componentObjsPath = scanInfoItem.ComponentObjsPath;
                            List<string> componentObjsName = scanInfoItem.ComponentObjsName;
                            string scanLog = scanInfoItem.ScanLog;

                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"������� => ({scanLog})");

                            bool isComponentObjFoldout = isComponentObjsFoldout;
                            if (GUILayout.Button(isComponentObjFoldout ? $"Foldout({componentObjsName.Count})" : $"Unfold({componentObjsName.Count})"))
                            {
                                scanInfoItem.IsComponentObjsFoldout = !scanInfoItem.IsComponentObjsFoldout;
                            }
                            GUILayout.EndHorizontal();

                            int indentLevel = 2;
                            if (!isComponentObjsFoldout)
                            {
                                for (int j = 0; j < componentObjsPath.Count; j++)
                                {
                                    GUILayout.BeginHorizontal();
                                    EditorGUI.indentLevel += indentLevel;
                                    EditorGUILayout.TextField("�������", componentObjsName[j]);
                                    EditorGUILayout.TextField("���·����", componentObjsPath[j]);
                                    EditorGUI.indentLevel -= indentLevel;
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }
                    }
                    GUILayout.Space(20);
                }
                GUILayout.EndScrollView();
            }
        }

        private GameObject InstantiateInHierarchy(string prefabPath, string prefabName)
        {
            GameObject destoryObj = null;
            do
            {
                if (destoryObj != null)
                {
                    UnityEngine.Object.DestroyImmediate(destoryObj);
                }
                destoryObj = GameObject.Find(prefabName);
            } while (destoryObj != null);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            // PrefabUtility.InstantiatePrefab ��Ԥ���屣�ֹ���, �Ա�ʹ��PrefabUtility.GetPrefabParent��ȡ����Ӧ��Ԥ����
            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            return go;
        }

        private void ForceRepair(List<ScanInfoItem<T>> scanInfoItems)
        {
            for (int index = 0; index < scanInfoItems.Count; index++)
            {
                ScanInfoItem<T> scanInfoItem = scanInfoItems[index];
                GameObject go = InstantiateInHierarchy(scanInfoItem.PrefabPath, scanInfoItem.PrefabName);
                Action<T> handle = scanInfoItem.Handle;
                T[] ts = go.GetComponentsInChildren<T>(true);
                for (int i = 0; i < ts.Length; i++)
                {
                    T t = ts[i];
                    string relativePath = GetRelativePathOfComponentOfPrefab(t, go.transform);
                    if (scanInfoItem.ComponentObjsPath.Contains(relativePath))
                    {
                        handle.Invoke(t);
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, scanInfoItem.PrefabPath, InteractionMode.AutomatedAction);
                // ��ʱ
                // PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
    }
}