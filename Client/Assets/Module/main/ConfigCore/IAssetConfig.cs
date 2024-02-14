using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public interface IAssetConfig
    {
        // 构建环境
        Environment BuildEnv();
        
        // 运行环境
        Environment RuntimeEnv();
        
        // 模块列表Url
        string Url();
        
        // 热更AB包，随游戏启动下载
        ReadOnlyCollection<string> RemoteGroup();
        
        // 运行Dll脚本的模式
        ReadOnlyCollection<Environment> DllRuntimeEnv();
    }
}