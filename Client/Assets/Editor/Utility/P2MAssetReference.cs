using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ninth.Editor
{
    [Serializable]
    public class P2MAssetReference<TParent, TParentConfig, TChild, TChildConfig>
        where TParent: UnityEngine.Object
        where TParentConfig: ParentConfig, new()
        where TChild: UnityEngine.Object
        where TChildConfig: ChildConfig, new()
    {
        public List<AssetReference<TParent, TParentConfig>> Parents = new();
        public List<AssetReference<TChild, TChildConfig>> Childs = new();
        public int DefaultParentWeight;
        public int DefaultChildWeight;

        public void ChildAdd(AssetReference<TParent, TParentConfig> parentRef, TChild asset, Action<TChildConfig> extraInit)
        {
            if (Childs.Select(x => x.Asset).ToList().Contains(asset))
            {
                "已存在资源, 请检查代码".FrameError();
                return;
            }
            var parentIndex = Parents.IndexOf(parentRef);
            var config = new TChildConfig
            {
                Weight = DefaultChildWeight,
                ParentRefIndexCollect = new() { parentIndex }
            };
            extraInit.Invoke(config);
            Childs.Add(new AssetReference<TChild, TChildConfig>
            {
                Asset = asset,
                Config = config
            });
            parentRef.Config.ChildRefIndexCollect.Add(Childs.Count - 1);
        }

        public void Bind(int parentIndex, int childIndex)
        {
            var parentCollect = Childs[childIndex].Config.ParentRefIndexCollect;
            var childCollect = Parents[parentIndex].Config.ChildRefIndexCollect;
            if (parentCollect.Contains(parentIndex) || childCollect.Contains(childIndex))
            {
                "无法在同一个 Parent 挂载 同一个 Child".Error();
                return;
            }
            Parents[parentIndex].Config.ChildRefIndexCollect.Add(childIndex);
            Childs[childIndex].Config.ParentRefIndexCollect.Add(parentIndex);
        }

        public void Remove(AssetReference<TParent, TParentConfig> parentRef, AssetReference<TChild, TChildConfig> childRef)
        {
            var parentIndex = Parents.IndexOf(parentRef);
            var childIndex = Childs.IndexOf(childRef);
            parentRef.Config.ChildRefIndexCollect.Remove(childIndex);
            childRef.Config.ParentRefIndexCollect.Remove(parentIndex);
            if (childRef.Config.ParentRefIndexCollect.Count == 0)
            {
                ChildRemoveAt(childIndex);
            }
        }
        
        public void ChildRemoveAt(int index)
        {
            Childs.RemoveAt(index);
            for(var i = 0; i < Parents.Count; i++)
            {
                var parent = Parents[i];
                var removeIndexStack = new Stack<int>();
                for (var j = 0; j < parent.Config.ChildRefIndexCollect.Count; j++)
                {
                    if (parent.Config.ChildRefIndexCollect[j] == index)
                    {
                        removeIndexStack.Push(j);
                    }
                    else if(parent.Config.ChildRefIndexCollect[j] > index)
                    {
                        parent.Config.ChildRefIndexCollect[j]--;
                    }
                }
                while (removeIndexStack.Count > 0)
                {
                    parent.Config.ChildRefIndexCollect.RemoveAt(removeIndexStack.Pop());
                }
            }
        }

        public void ParentAdd(TParent asset, Action<TParentConfig> extraInit)
        {
            var config = new TParentConfig
            {
                Weight = DefaultParentWeight
            };
            extraInit.Invoke(config);
            Parents.Add(new AssetReference<TParent, TParentConfig>
            {
                Asset = asset,
                Config = config
            });
        }
        
        public void ParentRemoveAt(int index)
        {
            Parents.RemoveAt(index);
            var removeChildIndexStack = new Stack<int>();
            for(var i = 0; i < Childs.Count; i++)
            {
                var child = Childs[i];
                var removeIndexStack = new Stack<int>();
                for (var j = 0; j < child.Config.ParentRefIndexCollect.Count; j++)
                {
                    if (child.Config.ParentRefIndexCollect[j] == index)
                    {
                        removeIndexStack.Push(j);
                    }
                    else if(child.Config.ParentRefIndexCollect[j] > index)
                    {
                        child.Config.ParentRefIndexCollect[j]--;
                    }
                }
                while (removeIndexStack.Count > 0)
                {
                    child.Config.ParentRefIndexCollect.RemoveAt(removeIndexStack.Pop());
                    if (child.Config.ParentRefIndexCollect.Count == 0)
                    {
                        removeChildIndexStack.Push(i);
                    }
                }
            }
            while (removeChildIndexStack.Count > 0)
            {
                Childs.RemoveAt(removeChildIndexStack.Pop());
            }
        }
    }

    [Serializable]
    public class ParentConfig
    {
        public int Weight;
        public List<int> ChildRefIndexCollect = new();
        public bool IsLock;
    }
    
    [Serializable]
    public class ChildConfig
    {
        public int Weight;
        public List<int> ParentRefIndexCollect = new();
        public bool IsRepeat;
        public bool IsLock;
    }
}