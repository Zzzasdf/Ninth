using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public interface IAssetConfig
    {
        // 运行环境
        Environment RuntimeEnv();
        
        // 模块列表Url
        string Url();
        
        // 运行Dll脚本的模式
        ReadOnlyCollection<Environment> DllRuntimeEnv();
    }
}