using System;
using System.Collections.Generic;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using NPOI.POIFS.Properties;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ninth.Editor
{
    [Serializable]
    public class P2MAssetReference<TParent, TChild>
    {
        public List<AssetReference<TParent, ParentConfig>> Parents = new();
        public List<AssetReference<TChild, ChildConfig>> Childs = new();
        public int DefaultParentWeight;
        public int DefaultChildWeight;

        public void ChildAdd(AssetReference<TParent, ParentConfig> parentRef, TChild asset)
        {
            if (Childs.Select(x => x.Asset).ToList().Contains(asset))
            {
                "已存在资源, 请检查代码".FrameError();
                return;
            }
            var parentIndex = Parents.IndexOf(parentRef);
            Childs.Add(new AssetReference<TChild, ChildConfig>
            {
                Asset = asset,
                Config = new ChildConfig
                {
                    Weight = DefaultChildWeight,
                    ParentRefIndexCollect = new (){ parentIndex }
                }
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

        public void ChildRemove(AssetReference<TParent, ParentConfig> parentRef, AssetReference<TChild, ChildConfig> childRef)
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

        public void ParentAdd(TParent asset)
        {
            Parents.Add(new AssetReference<TParent, ParentConfig>
            {
                Asset = asset,
                Config = new ParentConfig
                {
                    Weight = DefaultParentWeight
                }
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
    }
    
    [Serializable]
    public class ChildConfig
    {
        public int Weight;
        public List<int> ParentRefIndexCollect = new();
    }
}