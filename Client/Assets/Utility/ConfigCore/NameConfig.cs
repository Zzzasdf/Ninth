using System;
using UnityEngine;

namespace Ninth.Utility
{
    [CreateAssetMenu(fileName = "NameConfigSO", menuName = "Config/NameConfigSO")]
    [Serializable]
    public sealed class NameConfig: ScriptableObject, INameConfig
    {
        [SerializeField] private string fileNameByVersionConfig;
        [SerializeField] private string fileTempNameByVersionConfig;
        
        [SerializeField] private string directoryNameByLocalGroup;
        [SerializeField] private string loadConfigNameByLocalGroup;
        
        [SerializeField] private string directoryNameByRemoteGroup;
        [SerializeField] private string downloadConfigNameByRemoteGroup;
        [SerializeField] private string downloadConfigTempNameByRemoteGroup;
        [SerializeField] private string loadConfigNameByRemoteGroup;

        [SerializeField] private string directoryNameByDllGroup;
        [SerializeField] private string dllsBundleNameByDllGroup;
        [SerializeField] private string downloadConfigNameByDllGroup;
        [SerializeField] private string downloadConfigTempNameByDllGroup;
        [SerializeField] private string loadConfigNameByDllGroup;
        [SerializeField] private string directoryTempNameByPack;
        
        public NameConfig()
        {
            fileNameByVersionConfig = "VersionConfig.json";
            fileTempNameByVersionConfig = "VersionConfigTemp.json";

            directoryNameByLocalGroup = "Local";
            loadConfigNameByLocalGroup = "LoadConfigByLocalGroup.json";

            directoryNameByRemoteGroup = "Remote";
            downloadConfigNameByRemoteGroup = "DownloadConfigByRemoteGroup.json";
            downloadConfigTempNameByRemoteGroup = "DownloadConfigTempByRemoteGroup.json";
            loadConfigNameByRemoteGroup = "LoadConfigByRemoteGroup.json";

            directoryNameByDllGroup = "Dll";
            dllsBundleNameByDllGroup = "HotUpdateAssemblyByDllGroup";
            downloadConfigNameByDllGroup = "DownloadConfigByDllGroup.json";
            downloadConfigTempNameByDllGroup = "DownloadConfigTempByDllGroup.json";
            loadConfigNameByDllGroup = "LoadConfigByDllGroup.json";

            directoryTempNameByPack = "DirectoryTempByPack";
        }

        string INameConfig.FileNameByVersionConfig() => fileNameByVersionConfig;
        string INameConfig.FileTempNameByVersionConfig() => fileTempNameByVersionConfig;
        
        string INameConfig.FolderByLocalGroup() => directoryNameByLocalGroup;
        string INameConfig.LoadConfigNameByLocalGroup() => loadConfigNameByLocalGroup;
        
        string INameConfig.FolderByRemoteGroup() => directoryNameByRemoteGroup;
        string INameConfig.DownloadConfigNameByRemoteGroup() => downloadConfigNameByRemoteGroup;
        string INameConfig.DownloadConfigTempNameByRemoteGroup() => downloadConfigTempNameByRemoteGroup;
        string INameConfig.LoadConfigNameByRemoteGroup() => loadConfigNameByRemoteGroup;
        
        string INameConfig.FolderByDllGroup() => directoryNameByDllGroup;
        string INameConfig.DllsBundleNameByDllGroup() => dllsBundleNameByDllGroup;
        string INameConfig.DownloadConfigNameByDllGroup() => downloadConfigNameByDllGroup;
        string INameConfig.DownloadConfigTempNameByDllGroup() => downloadConfigTempNameByDllGroup;
        string INameConfig.LoadConfigNameByDllGroup() => loadConfigNameByDllGroup;

        string INameConfig.DirectoryTempNameByPack() => directoryTempNameByPack;

    }
}
