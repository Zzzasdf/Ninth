using System;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public abstract class BaseChildView : MonoBehaviour, INode
    {
        private IViewProxy viewProxy;
        public Type Type { get; set; }
        public int UniqueId { get; set; }
        public int Weight { get; set; }
        public int OrderId { get; set; }
        
        public void InitOnCreate(IViewProxy viewProxy, Type type, int weight)
        {
            this.viewProxy = viewProxy;
            this.Type = type;
            this.Weight = weight;
        }
    }
}
