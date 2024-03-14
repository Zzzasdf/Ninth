using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface INameConfig
    {
        // 版本配置文件名
        string FileNameByVersionConfig();
        // 版本配置临时文件名
        string FileTempNameByVersionConfig();
        
        // Local组文件夹名
        string FolderByLocalGroup();
        // Local组的加载配置名
        string LoadConfigNameByLocalGroup();

        // Remote组文件夹名
        string FolderByRemoteGroup();
        // Remote组的下载配置名
        string DownloadConfigNameByRemoteGroup();
        // Remote组的下载临时配置名
        string DownloadConfigTempNameByRemoteGroup();
        // Remote组的加载配置名
        string LoadConfigNameByRemoteGroup();

        // Dll文件夹
        string FolderByDllGroup();
        // Dll程序集集合的Bundle名
        string DllsBundleNameByDllGroup();
        // Dll的下载配置名
        string DownloadConfigNameByDllGroup();
        // Dll的下载临时配置名
        string DownloadConfigTempNameByDllGroup();
        // Dll的加载配置名
        string LoadConfigNameByDllGroup();
        
        // 打包的临时目录
        string DirectoryTempNameByPack();
    }
}
