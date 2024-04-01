using UnityEngine;

namespace Ninth.HotUpdate
{
    public abstract class BaseRole: MonoBehaviour, INode
    {
        public int UniqueId { get; set; }
        public int Weight { get; set; }
        public int OrderId { get; set; }
    }
}
