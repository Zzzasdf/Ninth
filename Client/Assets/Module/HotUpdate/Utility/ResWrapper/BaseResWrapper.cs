using UnityEngine;

namespace Ninth.HotUpdate
{
    // public enum ResQuality
    // {
    //     Default = 1,
    //     Low = 0,
    //     Medium = 1,
    //     High = 2,
    // }

    public abstract class BaseResWrapper<T>: MonoBehaviour
        where T: Component
    {
        [SerializeField] protected T originalComponent;

        // 渲染资源
        protected virtual void RenderRes()
        {
            
        }

        // 清空渲染资源
        protected virtual void ClearRenderRes()
        {

        }
    }
}