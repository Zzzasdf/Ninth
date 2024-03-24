using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ninth.HotUpdate
{
    public abstract class BaseView: MonoBehaviour, INode
    {
        private IViewProxy viewProxy;
        public Type Type { get; set; }
        public VIEW_HIERARCHY Hierarchy { get; set; }
        public int UniqueId { get; set; }
        public int Weight { get; set; }
        public int OrderId { get; set; }

        public void CreateInit(IViewProxy viewProxy, Type type, VIEW_HIERARCHY hierarchy, int weight)
        {
            this.viewProxy = viewProxy;
            this.Type = type;
            this.Hierarchy = hierarchy;
            this.Weight = weight;
        }

        public void AddUniqueId(int uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        public void Recycle(CancellationToken cancellationToken = default)
        {
            viewProxy.RecycleAsync(Hierarchy, UniqueId, cancellationToken);
        }
    }
}
