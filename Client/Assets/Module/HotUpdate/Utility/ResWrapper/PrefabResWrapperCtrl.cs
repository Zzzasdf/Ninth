using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth.HotUpdate
{
    // 预制体节点向下资源控制器
    public class PrefabResWrapperCtrl: MonoBehaviour
    {
        // [SerializeField] private ResQuality resQuality; // 界面拓展，监听改变

        // // 总节点下发派发质量
        // public void SetResQualiy(ResQuality resQuality)
        // {
        //     this.resQuality = resQuality;
        // }

        // // 向下渲染所有的资源
        // private void RenderResInChildren(ResQuality resQuality)
        // {
        //     BaseResWrapper[] resWrappers = GetComponentsInChildren<BaseResWrapper>();
        //     foreach(var resWrapper in resWrappers)
        //     {
        //         resWrapper.RenderRes(resQuality);
        //     }
        // }

        // // 向下清空所有的资源
        // private void ClearRenderRes()
        // {
        //     BaseResWrapper[] resWrappers = GetComponentsInChildren<BaseResWrapper>();
        //     foreach(var resWrapper in resWrappers)
        //     {
        //         resWrapper.ClearRenderRes();
        //     }
        // }
    }
}
