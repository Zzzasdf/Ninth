// TODO => 标签预警
// 导出/导入路径
// ViewProxy 补全

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class BaseAssetModuleProxy<TParent, TChild>: IOnGUI
        where TParent: MonoBehaviour
        where TChild: MonoBehaviour
    {
        protected BaseAssetModuleConfig<TParent, TChild> baseAssetModuleConfig;
        private GUIStyle frameBox = GUI.skin.GetStyle("FrameBox");
        
        public void OnGUI()
        {
            using (new GUILayout.HorizontalScope(frameBox))
            {
                RenderSelectFolderCollect();
                RenderBtnCollect();
            }

            using (new EditorGUILayout.VerticalScope(frameBox))
            {
                RenderParentCollect(); 
            }

            using (new EditorGUILayout.VerticalScope())
            {
                var selector = baseAssetModuleConfig.HierarchyInfo.Selector;
                selector.CurrentIndex.Value = GUILayout.Toolbar(selector.CurrentIndex.Value, selector.Keys.Select(x => x.ToString()).ToArray());
                var secondSelector = selector.CurrentValue;
                secondSelector.CurrentIndex.Value = GUILayout.Toolbar(secondSelector.CurrentIndex.Value, secondSelector.Collect.Select(x => x.ToString()).ToArray());
                
                switch (selector.Current.Value)
                {
                    case FirstHierarchy.Parent:
                    {
                        switch (secondSelector.Current.Value)
                        {
                            case SecondHierarchy.Appointed: RenderAppointedParent(); break;
                            case SecondHierarchy.UnAppoint: RenderUnAppointParent(); break;
                        }
                        break;
                    }
                    case FirstHierarchy.Child:
                    {
                        switch (secondSelector.Current.Value)
                        {
                            case SecondHierarchy.Appointed: RenderAppointedChild(); break;
                            case SecondHierarchy.UnAppoint: RenderUnAppointChild(); break;
                            case SecondHierarchy.RepeatAppoint: RenderRepeatAppointChild(); break;
                        }
                        break;
                    }
                }
            }
        }

        private void RenderSelectFolderCollect()
        {
            var folders = baseAssetModuleConfig.AssetFolders;
            using (new EditorGUILayout.VerticalScope(frameBox))
            {
                var removeIndexStack = new Stack<int>();
                for (var i = 0; i < folders.Count; i++)
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUI.enabled = false;
                            EditorGUILayout.TextField($"[{i}] =>", folders[i].RelativePath);
                            GUI.enabled = true;
                            if (GUILayout.Button("浏览"))
                            {
                                var folderModify = EditorUtility.OpenFolderPanel("选择目标文件夹", folders[i].FullPath, string.Empty);
                                folders.TryAdd(folderModify);
                            }
                            if (GUILayout.Button("移除"))
                            {
                                removeIndexStack.Push(i);
                            }
                        }
                    }
                }
                while (removeIndexStack.Count > 0)
                {
                    var index = removeIndexStack.Pop();
                    folders.RemoveAt(index);
                }
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("新增文件夹"))
                    {
                        var newFolder = string.Empty;
                        newFolder = EditorUtility.OpenFolderPanel("选择目标文件夹", newFolder, string.Empty);
                        folders.TryAdd(newFolder);
                    }
                }
            }
        }

        private void RenderBtnCollect()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (GUILayout.Button("导入"))
                {
                    "TODO => 导入 json".Log();
                }
                if (GUILayout.Button("一键删除Miss{0}"))
                {
                    "TODO => 二次询问".Log();
                }
                if (GUILayout.Button("清空"))
                {
                    "TODO => 二次询问".Log();
                    // baseAssetModuleConfig.P2MAssetReference.Parents.Clear();
                    // baseAssetModuleConfig.P2MAssetReference.Childs.Clear();
                }
                if (GUILayout.Button("保存"))
                {
                    EditorUtility.SetDirty(baseAssetModuleConfig);
                    AssetDatabase.SaveAssetIfDirty(baseAssetModuleConfig);
                    "TODO => 转换成 json".Log();
                }
            }
        }

        private void RenderParentCollect()
        {
            var removeIndexStack = new Stack<int>();
            for (var i = 0; i < baseAssetModuleConfig.P2MAssetReference.Parents.Count; i++)
            {
                var parentRef = baseAssetModuleConfig.P2MAssetReference.Parents[i];
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"[{i}] =>");
                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(parentRef.Asset, typeof(TParent), false);
                            parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                            if (GUILayout.Button("Remove"))
                            {
                                removeIndexStack.Push(i);
                            }
                        }
                        if (parentRef.Config.ChildRefIndexCollect.Count != 0)
                        {
                            RenderChildCollect(parentRef);
                        }
                    }
                }
            }
            while (removeIndexStack.Count > 0)
            {
                baseAssetModuleConfig.P2MAssetReference.ParentRemoveAt(removeIndexStack.Pop());
            }
            return;
            
            void RenderChildCollect(AssetReference<TParent, ParentConfig> parentRef)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    var removeIndexStack = new Stack<int>();
                    for (var i = 0; i < parentRef.Config.ChildRefIndexCollect.Count; i++)
                    {
                        var childRef = baseAssetModuleConfig.P2MAssetReference.Childs[parentRef.Config.ChildRefIndexCollect[i]];
                        using (new EditorGUILayout.VerticalScope())
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label($"[{i}] =>");
                                EditorGUILayout.ObjectField(childRef.Asset, typeof(TChild), false);
                                childRef.Config.Weight = EditorGUILayout.IntField(childRef.Config.Weight);
                                if (GUILayout.Button("Remove"))
                                {
                                    removeIndexStack.Push(i);
                                }
                            }
                        }
                    }
                    while (removeIndexStack.Count > 0)
                    {
                        var childRefIndex = parentRef.Config.ChildRefIndexCollect[removeIndexStack.Pop()];
                        var childRef = baseAssetModuleConfig.P2MAssetReference.Childs[childRefIndex];
                        baseAssetModuleConfig.P2MAssetReference.ChildRemove(parentRef, childRef);
                    }
                }
            }
        }

        private void RenderAppointedParent()
        {
            var removeIndexStack = new Stack<int>();
            for (var i = 0; i < baseAssetModuleConfig.P2MAssetReference.Parents.Count; i++)
            {
                var parentRef = baseAssetModuleConfig.P2MAssetReference.Parents[i];
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"[{i}] =>");
                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(parentRef.Asset, typeof(TParent), false);
                            parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                            if (GUILayout.Button("Remove"))
                            {
                                removeIndexStack.Push(i);
                            }
                        }
                    }
                }
            }
            while (removeIndexStack.Count > 0)
            {
                baseAssetModuleConfig.P2MAssetReference.ParentRemoveAt(removeIndexStack.Pop());
            }
        }
        
        private void RenderUnAppointParent()
        {
            using (new EditorGUILayout.VerticalScope(frameBox))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("权重");
                    baseAssetModuleConfig.P2MAssetReference.DefaultParentWeight = EditorGUILayout.IntField(baseAssetModuleConfig.P2MAssetReference.DefaultParentWeight);
                }
                foreach (var folder in baseAssetModuleConfig.AssetFolders)
                {
                    var folderInfo = new DirectoryInfo(folder.FullPath);
                    var fileInfos = folderInfo.GetFiles();
                    foreach (var fileInfo in fileInfos)
                    {
                        var assetPath = new AssetPath();
                        if (!assetPath.IsVerify(fileInfo.FullName))
                        {
                            continue;
                        }
                        assetPath.TrySetFullPath(fileInfo.FullName);
                        var asset = AssetDatabase.LoadAssetAtPath<TParent>(assetPath.RelativePath);
                        if (asset == null)
                        {
                            continue;
                        }
                        if (baseAssetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset).ToList().Contains(asset))
                        {
                            continue;
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(asset, typeof(TParent), false);
                            if (GUILayout.Button("添加"))
                            {
                                baseAssetModuleConfig.P2MAssetReference.ParentAdd(asset);
                            }
                        }
                    }
                }
            }
        }

        private void RenderAppointedChild()
        {
            var removeIndexStack = new Stack<int>();
            var addIndexQueue = new Queue<(int parentIndex, int childIndex)>();
            for (var i = 0; i < baseAssetModuleConfig.P2MAssetReference.Childs.Count; i++)
            {
                var childRef = baseAssetModuleConfig.P2MAssetReference.Childs[i];
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label($"[{i}] =>");
                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(childRef.Asset, typeof(TParent), false);
                            childRef.Config.Weight = EditorGUILayout.IntField(childRef.Config.Weight);
                            if (GUILayout.Button("Remove"))
                            {
                                removeIndexStack.Push(i);
                            }
                            var stringList = new List<string> { "添加" };
                            stringList.AddRange(baseAssetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset != null ? $"{x.Asset.name}": "引用丢失").ToList());
                            var intList = new List<int>();
                            for (var j = 0; j < stringList.Count; j++)
                            {
                                intList.Add(j);
                            }
                            var duplicatesStack = new Stack<int>();
                            var childIndex = baseAssetModuleConfig.P2MAssetReference.Childs.IndexOf(childRef);
                            for (var j = 0; j < baseAssetModuleConfig.P2MAssetReference.Parents.Count; j++)
                            {
                                if (!baseAssetModuleConfig.P2MAssetReference.Parents[j].Config.ChildRefIndexCollect.Contains(childIndex))
                                {
                                    continue;
                                }
                                duplicatesStack.Push(j);
                            }
                            while (duplicatesStack.Count > 0)
                            {
                                var duplicatesIndex = duplicatesStack.Pop();
                                stringList.RemoveAt(duplicatesIndex + 1);
                                intList.RemoveAt(duplicatesIndex + 1);
                            }
                            var index = 0;
                            index = EditorGUILayout.IntPopup(index, stringList.ToArray(), intList.ToArray());
                            if (index != 0)
                            {
                                addIndexQueue.Enqueue((index - 1, i));
                            }
                        }
                        RenderRefParent(childRef);
                    }
                }
            }
            while (removeIndexStack.Count > 0)
            {
                baseAssetModuleConfig.P2MAssetReference.ChildRemoveAt(removeIndexStack.Pop());
            }

            while (addIndexQueue.Count > 0)
            {
                var (parentIndex, childIndex) = addIndexQueue.Dequeue();
                baseAssetModuleConfig.P2MAssetReference.Bind(parentIndex, childIndex);
            }
            return;

            void RenderRefParent(AssetReference<TChild, ChildConfig> childRef)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    var removeIndexStack = new Stack<int>();
                    for (var i = 0; i < childRef.Config.ParentRefIndexCollect.Count; i++)
                    {
                        var parentRef = baseAssetModuleConfig.P2MAssetReference.Parents[childRef.Config.ParentRefIndexCollect[i]];
                        using (new EditorGUILayout.VerticalScope())
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label($"[{i}] =>");
                                EditorGUILayout.ObjectField(parentRef.Asset, typeof(TChild), false);
                                parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                                if (GUILayout.Button("Remove"))
                                {
                                    removeIndexStack.Push(i);
                                }
                            }
                        }
                    }
                    while (removeIndexStack.Count > 0)
                    {
                        var parentIndex = childRef.Config.ParentRefIndexCollect[removeIndexStack.Pop()];
                        var parentRef = baseAssetModuleConfig.P2MAssetReference.Parents[parentIndex];
                        baseAssetModuleConfig.P2MAssetReference.ChildRemove(parentRef, childRef);
                    }
                }
            }
        }

        private void RenderUnAppointChild()
        {
            using (new EditorGUILayout.VerticalScope(frameBox))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("权重");
                    baseAssetModuleConfig.P2MAssetReference.DefaultChildWeight = EditorGUILayout.IntField(baseAssetModuleConfig.P2MAssetReference.DefaultChildWeight);
                }
                foreach (var folder in baseAssetModuleConfig.AssetFolders)
                {
                    var folderInfo = new DirectoryInfo(folder.FullPath);
                    var fileInfos = folderInfo.GetFiles();
                    foreach (var fileInfo in fileInfos)
                    {
                        var assetPath = new AssetPath();
                        if (!assetPath.IsVerify(fileInfo.FullName))
                        {
                            continue;
                        }
                        assetPath.TrySetFullPath(fileInfo.FullName);
                        var asset = AssetDatabase.LoadAssetAtPath<TChild>(assetPath.RelativePath);
                        if (asset == null)
                        {
                            continue;
                        }
                        if (baseAssetModuleConfig.P2MAssetReference.Childs.Select(x => x.Asset).ToList().Contains(asset))
                        {
                            continue;
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(asset, typeof(BaseChildView), false);
                            var stringList = new List<string> { "添加" };
                            stringList.AddRange(baseAssetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset != null ? $"{x.Asset.name}": "引用丢失").ToList());
                            var intList = new List<int>();
                            for (var i = 0; i < stringList.Count; i++)
                            {
                                intList.Add(i);
                            }
                            var index = 0;
                            index = EditorGUILayout.IntPopup(index, stringList.ToArray(), intList.ToArray());
                            if (index != 0)
                            {
                                baseAssetModuleConfig.P2MAssetReference.ChildAdd(baseAssetModuleConfig.P2MAssetReference.Parents[index - 1], asset);
                            }
                        }
                    }
                }
            }
        }

        private void RenderRepeatAppointChild()
        {
            using (new EditorGUILayout.VerticalScope(frameBox))
            {
                "TODO RepeatAppointChild".Log();
            }
        }
    }
}
