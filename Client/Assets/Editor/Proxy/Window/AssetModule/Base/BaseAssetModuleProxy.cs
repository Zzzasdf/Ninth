using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public abstract class BaseAssetModuleProxy<TParent, TParentConfig, TChild, TChildConfig, TP2MAssetReference, TAssetModuleConfig>: IOnGUI
        where TParent: UnityEngine.Object
        where TParentConfig: ParentConfig, new()
        where TChild: UnityEngine.Object
        where TChildConfig: ChildConfig, new()
        where TP2MAssetReference: P2MAssetReference<TParent, TParentConfig, TChild, TChildConfig>, new()
        where TAssetModuleConfig: BaseAssetModuleConfig<TParent, TParentConfig, TChild, TChildConfig, TP2MAssetReference>, new()
    {
        private GUIStyle frameBox = GUI.skin.GetStyle("FrameBox");

        protected TAssetModuleConfig assetModuleConfig;

        protected abstract bool RenderLockSOStatus();
        protected abstract void ModifyLockStatus();
        
        protected virtual void RenderDefaultParentExtraConfig(TP2MAssetReference p2MAssetReference) { }
        protected virtual void ParentExtraInit(TParentConfig parentConfig) { }
        protected virtual void RenderParentExtraConfig(TParentConfig parentConfig) { }

        protected virtual void RenderDefaultChildExtraConfig(TP2MAssetReference p2MAssetReference) { }
        protected virtual void ChildExtraInit(TChildConfig childConfig) { }
        protected virtual void RenderChildExtraConfig(TChildConfig childConfig) { }
        
        protected virtual void RenderExtraConfig(TP2MAssetReference p2MAssetReference) { }
        
        public void OnGUI()
        {
            using (new GUILayout.HorizontalScope(frameBox))
            {
                if (assetModuleConfig != null)
                {
                    using (new GUILayout.VerticalScope(frameBox))
                    {
                        RenderExtraConfig(assetModuleConfig.P2MAssetReference);
                        RenderSelectFolderCollect();
                    }
                }
                using (new GUILayout.VerticalScope(frameBox))
                {
                    RenderAssetModuleConfigSO();
                    RenderBtnCollect();
                }
                if (assetModuleConfig == null) return;
            }
 
            using (new GUILayout.VerticalScope(frameBox))
            {
                RenderParentCollect(); 
            }
            using (new GUILayout.VerticalScope())
            {
                var warningDic = new Dictionary<FirstHierarchy, Dictionary<SecondHierarchy, int>>
                {
                    [FirstHierarchy.Parent] = new()
                    {
                        [SecondHierarchy.Appointed] = AppointedParent(Implement.MissCount),
                        [SecondHierarchy.UnAppoint] = UnAppointParent(Implement.Count)
                    },
                    [FirstHierarchy.Child] = new()
                    {
                        [SecondHierarchy.Appointed] = AppointedChild(Implement.MissCount),
                        [SecondHierarchy.UnAppoint] = UnAppointChild(Implement.Count),
                        [SecondHierarchy.RepeatAppoint] = RepeatAppointChild(Implement.Count)
                    },
                };
                
                var selector = assetModuleConfig.HierarchyInfo.Selector;
                selector.CurrentIndex.Value = GUILayout.Toolbar(selector.CurrentIndex.Value, 
                    selector.Keys.Select(x =>
                        {
                            var count = warningDic[x].Values.Sum();
                            return count == 0 ? x.ToString() : $"{x} ({count})";
                        }).ToArray());
                var secondSelector = selector.CurrentValue;
                secondSelector.CurrentIndex.Value = GUILayout.Toolbar(secondSelector.CurrentIndex.Value, 
                    secondSelector.Collect.Select(x =>
                    {
                        warningDic[selector.Current.Value].TryGetValue(x, out var count);
                        return count == 0 ? x.ToString() : $"{x} ({count})";
                    }).ToArray());
                
                switch (selector.Current.Value)
                {
                    case FirstHierarchy.Parent:
                    {
                        switch (secondSelector.Current.Value)
                        {
                            case SecondHierarchy.Appointed: AppointedParent(Implement.Render); break;
                            case SecondHierarchy.UnAppoint: UnAppointParent(Implement.Render); break;
                        }
                        break;
                    }
                    case FirstHierarchy.Child:
                    {
                        switch (secondSelector.Current.Value)
                        {
                            case SecondHierarchy.Appointed: AppointedChild(Implement.Render); break;
                            case SecondHierarchy.UnAppoint: UnAppointChild(Implement.Render); break;
                            case SecondHierarchy.RepeatAppoint: RepeatAppointChild(Implement.Render); break;
                        }
                        break;
                    }
                }
            }
        }

        private void RenderSelectFolderCollect()
        {
            var folders = assetModuleConfig.AssetFolders;
            var foldersRelativePath = folders.Select(x => x.RelativePath).ToList();
            assetModuleConfig.AssetFoldout = EditorGUILayout.Foldout(assetModuleConfig.AssetFoldout, "文件夹集");
            if (assetModuleConfig.AssetFoldout)
            {
                var addFullPathQueue = new Queue<string>();
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
                                folders.TryModify(i, folderModify);
                            }
                            var subFolders = Directory.GetDirectories(folders[i].FullPath, "*", SearchOption.AllDirectories);
                            var validSubFolders = new List<string>();
                            foreach (var subFolder in subFolders)
                            {
                                if (!foldersRelativePath.Contains(AssetPath.Convert2Relative(subFolder)))
                                {
                                    validSubFolders.Add(subFolder);
                                }
                            }
                            if (validSubFolders.Count > 0)
                            {
                                if (GUILayout.Button("一键添加子文件夹"))
                                {
                                    foreach (var folder in validSubFolders)
                                    {
                                        addFullPathQueue.Enqueue(folder);
                                    }
                                }
                            }
                            if (GUILayout.Button("移除"))
                            {
                                removeIndexStack.Push(i);
                            }
                        }
                    }
                }
                while (addFullPathQueue.Count > 0)
                {
                    folders.TryAdd(addFullPathQueue.Dequeue());
                }
                while (removeIndexStack.Count > 0)
                {
                    var index = removeIndexStack.Pop();
                    folders.RemoveAt(index);
                }
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
        
        private void RenderAssetModuleConfigSO()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = !RenderLockSOStatus();
                assetModuleConfig = (TAssetModuleConfig)EditorGUILayout.ObjectField(assetModuleConfig, typeof(TAssetModuleConfig));
                GUI.enabled = true;
                if (GUILayout.Button("固定"))
                {
                    ModifyLockStatus();
                }
            }
        }
        
        private void RenderBtnCollect()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (GUILayout.Button("保存"))
                {
                    EditorUtility.SetDirty(assetModuleConfig);
                    AssetDatabase.SaveAssetIfDirty(assetModuleConfig);
                }
                if (GUILayout.Button("导出 Json"))
                {
                    "TODO => 转换成 json".Log();
                }
            }
        }

        private void RenderParentCollect()
        {
            var removeIndexStack = new Stack<int>();
            for (var i = 0; i < assetModuleConfig.P2MAssetReference.Parents.Count; i++)
            {
                var parentRef = assetModuleConfig.P2MAssetReference.Parents[i];
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button($"[{i}]"))
                    {
                        parentRef.Config.IsLock = !parentRef.Config.IsLock;
                    }
                    using (new GUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUI.enabled = !parentRef.Config.IsLock;
                            EditorGUILayout.ObjectField(parentRef.Asset, typeof(TParent), false);
                            RenderParentExtraConfig(parentRef.Config);
                            parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                            if (GUILayout.Button("Remove"))
                            {
                                if(EditorUtility.DisplayDialog("删除！！", "删除该项, 是否继续?", "确认", "取消"))
                                    removeIndexStack.Push(i);
                            }
                            GUI.enabled = true;
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
                assetModuleConfig.P2MAssetReference.ParentRemoveAt(removeIndexStack.Pop());
            }
            return;
            
            void RenderChildCollect(AssetReference<TParent, TParentConfig> parentRef)
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    var removeIndexStack = new Stack<int>();
                    for (var i = 0; i < parentRef.Config.ChildRefIndexCollect.Count; i++)
                    {
                        var childRef = assetModuleConfig.P2MAssetReference.Childs[parentRef.Config.ChildRefIndexCollect[i]];
                        using (new EditorGUILayout.VerticalScope())
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button($"[{i}]"))
                                {
                                    childRef.Config.IsLock = !childRef.Config.IsLock;
                                }
                                GUI.enabled = !childRef.Config.IsLock;
                                EditorGUILayout.ObjectField(childRef.Asset, typeof(TChild), false);
                                RenderChildExtraConfig(childRef.Config);
                                childRef.Config.Weight = EditorGUILayout.IntField(childRef.Config.Weight);
                                if (GUILayout.Button("Remove"))
                                {
                                    if(EditorUtility.DisplayDialog("删除！！", "删除该项, 是否继续?", "确认", "取消"))
                                        removeIndexStack.Push(i);
                                }
                                GUI.enabled = true;
                            }
                        }
                    }
                    while (removeIndexStack.Count > 0)
                    {
                        var childRefIndex = parentRef.Config.ChildRefIndexCollect[removeIndexStack.Pop()];
                        var childRef = assetModuleConfig.P2MAssetReference.Childs[childRefIndex];
                        assetModuleConfig.P2MAssetReference.Remove(parentRef, childRef);
                    }
                }
            }
        }

        private int AppointedParent(Implement implement)
        {
            var result = 0;
            switch (implement)
            {
                case Implement.Count:
                case Implement.MissCount:
                {
                    result = Items(implement);
                    break;
                }
                case Implement.Render:
                {
                    using (new GUILayout.VerticalScope())
                    {
                        if (Items(Implement.MissCount) > 0 &&GUILayout.Button("一键移除Missing"))
                        {
                            Items(Implement.MissAutoRemove);
                        }
                        Items(implement);
                    }
                    break;
                }
            }
            return result;

            int Items(Implement implement)
            {
                var result = 0;
                var removeIndexStack = new Stack<int>();
                for (var i = 0; i < assetModuleConfig.P2MAssetReference.Parents.Count; i++)
                {
                    var parentRef = assetModuleConfig.P2MAssetReference.Parents[i];
                    switch (implement)
                    {
                        case Implement.Count:
                        {
                            result++;
                            break;
                        }
                        case Implement.MissCount:
                        {
                            if (parentRef.Asset == null)
                            {
                                result++;
                            }
                            break;
                        }
                        case Implement.MissAutoRemove:
                        {
                            if (parentRef.Asset == null)
                            {
                                removeIndexStack.Push(i);
                            }
                            break;
                        }
                        case Implement.Render:
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                if(GUILayout.Button($"[{i}]"))
                                {
                                    parentRef.Config.IsLock = !parentRef.Config.IsLock;
                                }
                                using (new GUILayout.VerticalScope())
                                {
                                    using (new GUILayout.HorizontalScope())
                                    {
                                        GUI.enabled = !parentRef.Config.IsLock;
                                        EditorGUILayout.ObjectField(parentRef.Asset, typeof(TParent), false);
                                        RenderParentExtraConfig(parentRef.Config);
                                        parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                                        if (GUILayout.Button("Remove"))
                                        {
                                            if(EditorUtility.DisplayDialog("删除！！", "删除该项, 是否继续?", "确认", "取消"))
                                                removeIndexStack.Push(i);
                                        }
                                        GUI.enabled = true;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                while (removeIndexStack.Count > 0)
                {
                    assetModuleConfig.P2MAssetReference.ParentRemoveAt(removeIndexStack.Pop());
                }
                return result;
            }
        }

        private int UnAppointParent(Implement implement)
        {
            var result = 0;
            switch (implement)
            {
                case Implement.Count:
                {
                    result = Items(Implement.Count);
                    break;
                }
                case Implement.Render:
                {
                    using (new EditorGUILayout.VerticalScope(frameBox))
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            RenderDefaultParentExtraConfig(assetModuleConfig.P2MAssetReference);
                            GUILayout.Label("权重");
                            assetModuleConfig.P2MAssetReference.DefaultParentWeight = EditorGUILayout.IntField(assetModuleConfig.P2MAssetReference.DefaultParentWeight);
                        }
                        if (Items(Implement.Count) > 0 && GUILayout.Button("一键导入"))
                        {
                            Items(Implement.AutoAppoint);
                        }
                        Items(implement);
                        break;
                    }
                }
            }
            return result;

            int Items(Implement implement)
            {
                var result = 0;
                foreach (var folder in assetModuleConfig.AssetFolders)
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
                        if (assetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset).ToList().Contains(asset))
                        {
                            continue;
                        }
                        result++;
                        switch (implement)
                        {
                            case Implement.Render:
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.ObjectField(asset, typeof(TParent), false);
                                    if (GUILayout.Button("添加"))
                                    {
                                        assetModuleConfig.P2MAssetReference.ParentAdd(asset, ParentExtraInit);
                                    }
                                }
                                break;
                            }
                            case Implement.AutoAppoint:
                            {
                                assetModuleConfig.P2MAssetReference.ParentAdd(asset, ParentExtraInit);
                                break;
                            }
                        }
                    }
                }
                return result;
            }
        }

        private int AppointedChild(Implement implement)
        {
            var result = 0;
            switch (implement)
            {
                case Implement.Count:
                case Implement.MissCount:
                {
                    result = Items(implement);
                    break;
                }
                case Implement.Render:
                {
                    using (new GUILayout.VerticalScope())
                    {
                        if (Items(Implement.MissCount) > 0 &&GUILayout.Button("一键移除Missing"))
                        {
                            Items(Implement.MissAutoRemove);
                        }
                        Items(implement);
                    }
                    break;
                }
            }
            return result;

            int Items(Implement implement)
            {
                var result = 0;
                var removeIndexStack = new Stack<int>();
                var addIndexQueue = new Queue<(int parentIndex, int childIndex)>();
                for (var i = 0; i < assetModuleConfig.P2MAssetReference.Childs.Count; i++)
                {
                    var childRef = assetModuleConfig.P2MAssetReference.Childs[i];
                    switch (implement)
                    {
                        case Implement.Count:
                        {
                            result++;
                            break;
                        }
                        case Implement.MissCount:
                        {
                            if (childRef.Asset == null)
                            {
                                result++;
                            }
                            break;
                        }
                        case Implement.MissAutoRemove:
                        {
                            if (childRef.Asset == null)
                            {
                                removeIndexStack.Push(i);
                            }
                            break;
                        }
                        case Implement.Render:
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button($"[{i}]"))
                                {
                                    childRef.Config.IsLock = !childRef.Config.IsLock;
                                }
                                using (new EditorGUILayout.VerticalScope())
                                {
                                    GUI.enabled = !childRef.Config.IsLock;
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        EditorGUILayout.ObjectField(childRef.Asset, typeof(TParent), false);
                                        RenderChildExtraConfig(childRef.Config);
                                        childRef.Config.Weight = EditorGUILayout.IntField(childRef.Config.Weight);
                                        if (!childRef.Config.IsRepeat)
                                        {
                                            if (GUILayout.Button("支持复用"))
                                            {
                                                childRef.Config.IsRepeat = true;
                                            }
                                        }
                                        else
                                        {
                                            if (GUILayout.Button("取消复用"))
                                            {
                                                if (childRef.Config.ParentRefIndexCollect.Count > 1)
                                                {
                                                    if (EditorUtility.DisplayDialog("取消复用", "取消复用将只保留第一个 Parent 的 引用, 是否继续?", "确认", "取消"))
                                                    {
                                                        while (childRef.Config.ParentRefIndexCollect.Count > 1)
                                                        {
                                                            var parentIndex = childRef.Config.ParentRefIndexCollect[^1];
                                                            var parentRef = assetModuleConfig.P2MAssetReference.Parents[parentIndex];
                                                            assetModuleConfig.P2MAssetReference.Remove(parentRef, childRef);
                                                        }

                                                        childRef.Config.IsRepeat = false;
                                                    }
                                                }
                                                else
                                                {
                                                    childRef.Config.IsRepeat = false;
                                                }
                                            }
                                        }
                                        if (childRef.Config.IsRepeat)
                                        {
                                            var stringList = new List<string> { "添加" };
                                            stringList.AddRange(assetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset != null ? $"{x.Asset.name}" : "引用丢失").ToList());
                                            var intList = new List<int>();
                                            for (var j = 0; j < stringList.Count; j++)
                                            {
                                                intList.Add(j);
                                            }
                                            var duplicatesStack = new Stack<int>();
                                            var childIndex = assetModuleConfig.P2MAssetReference.Childs.IndexOf(childRef);
                                            for (var j = 0; j < assetModuleConfig.P2MAssetReference.Parents.Count; j++)
                                            {
                                                if (!assetModuleConfig.P2MAssetReference.Parents[j].Config.ChildRefIndexCollect.Contains(childIndex))
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
                                        if (GUILayout.Button("Remove"))
                                        {
                                            if (EditorUtility.DisplayDialog("删除！！", "删除该项, 是否继续?", "确认", "取消"))
                                                removeIndexStack.Push(i);
                                        }
                                    }
                                    RenderRefParent(childRef);
                                    GUI.enabled = true;
                                }
                            }
                            break;
                        }
                    }
                }
                while (removeIndexStack.Count > 0)
                {
                    assetModuleConfig.P2MAssetReference.ChildRemoveAt(removeIndexStack.Pop());
                }

                while (addIndexQueue.Count > 0)
                {
                    var (parentIndex, childIndex) = addIndexQueue.Dequeue();
                    assetModuleConfig.P2MAssetReference.Bind(parentIndex, childIndex);
                }
                return result;

                void RenderRefParent(AssetReference<TChild, TChildConfig> childRef)
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        var removeIndexStack = new Stack<int>();
                        for (var i = 0; i < childRef.Config.ParentRefIndexCollect.Count; i++)
                        {
                            var parentRef = assetModuleConfig.P2MAssetReference.Parents[childRef.Config.ParentRefIndexCollect[i]];
                            using (new EditorGUILayout.VerticalScope())
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    if (GUILayout.Button($"[{i}]"))
                                    {
                                        parentRef.Config.IsLock = !parentRef.Config.IsLock;
                                    }
                                    EditorGUILayout.ObjectField(parentRef.Asset, typeof(TChild), false);
                                    RenderParentExtraConfig(parentRef.Config);
                                    parentRef.Config.Weight = EditorGUILayout.IntField(parentRef.Config.Weight);
                                    if (GUILayout.Button("Remove"))
                                    {
                                        if (EditorUtility.DisplayDialog("删除！！", "删除该项, 是否继续?", "确认", "取消"))
                                            removeIndexStack.Push(i);
                                    }
                                }
                            }
                        }

                        while (removeIndexStack.Count > 0)
                        {
                            var parentIndex = childRef.Config.ParentRefIndexCollect[removeIndexStack.Pop()];
                            var parentRef = assetModuleConfig.P2MAssetReference.Parents[parentIndex];
                            assetModuleConfig.P2MAssetReference.Remove(parentRef, childRef);
                        }
                    }
                }
            }
        }

        private int UnAppointChild(Implement implement)
        {
            var result = Items(Implement.Count);
            switch (implement)
            {
                case Implement.Render:
                {
                    using (new EditorGUILayout.VerticalScope(frameBox))
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            RenderDefaultChildExtraConfig(assetModuleConfig.P2MAssetReference);
                            GUILayout.Label("权重");
                            assetModuleConfig.P2MAssetReference.DefaultChildWeight = EditorGUILayout.IntField(assetModuleConfig.P2MAssetReference.DefaultChildWeight);
                        }
                        Items(Implement.Render);
                    }
                    break;
                }
            }
            return result;

            int Items(Implement implement)
            {
                var result = 0;
                foreach (var folder in assetModuleConfig.AssetFolders)
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
                        if (assetModuleConfig.P2MAssetReference.Childs.Select(x => x.Asset).ToList().Contains(asset))
                        {
                            continue;
                        }
                        result++;
                        switch (implement)
                        {
                            case Implement.Render:
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.ObjectField(asset, typeof(BaseChildView), false);
                                    var stringList = new List<string> { "添加" };
                                    stringList.AddRange(assetModuleConfig.P2MAssetReference.Parents.Select(x => x.Asset != null ? $"{x.Asset.name}" : "引用丢失").ToList());
                                    var intList = new List<int>();
                                    for (var i = 0; i < stringList.Count; i++)
                                    {
                                        intList.Add(i);
                                    }
                                    var index = 0;
                                    index = EditorGUILayout.IntPopup(index, stringList.ToArray(), intList.ToArray());
                                    if (index != 0)
                                    {
                                        assetModuleConfig.P2MAssetReference.ChildAdd(assetModuleConfig.P2MAssetReference.Parents[index - 1], asset, ChildExtraInit);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                return result;
            }
        }

        private int RepeatAppointChild(Implement implement)
        {
            var result = RenderItems(Implement.Count);
            switch (implement)
            {
                case Implement.Render:
                {
                    using (new EditorGUILayout.VerticalScope(frameBox))
                    {
                        if (result > 0 && GUILayout.Button("一键优化"))
                        {
                            RenderItems(Implement.AutoAppoint);
                        }
                        RenderItems(implement);
                    }
                    break;
                }
            }
            return result;

            int RenderItems(Implement implement)
            {
                var result = 0;
                for (var i = 0; i < assetModuleConfig.P2MAssetReference.Childs.Count; i++)
                {
                    var childRef = assetModuleConfig.P2MAssetReference.Childs[i];
                    if (!childRef.Config.IsRepeat)
                    {
                        continue;
                    }
                    var repeatCount = childRef.Config.ParentRefIndexCollect.Count;
                    if (repeatCount == 1)
                    {
                        result++;
                    }
                    switch (implement)
                    {
                        case Implement.Render:
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button($"[{i}]"))
                                {
                                    childRef.Config.IsLock = !childRef.Config.IsLock;
                                }
                                EditorGUILayout.ObjectField(childRef.Asset, typeof(TChild), false);
                                GUILayout.Label($"复用数量：{repeatCount}");
                                if (repeatCount == 1 && GUILayout.Button("取消复用"))
                                {
                                    childRef.Config.IsRepeat = false;
                                }
                            }
                            break;
                        }
                        case Implement.AutoAppoint:
                        {
                            childRef.Config.IsRepeat = false;
                            break;
                        }
                    }
                }
                return result;
            }
        }
    }
}
