using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ninth
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
            downloadConfigTempNameByDllGroup = "DownloadConfigByDllGroup.json";
            loadConfigNameByDllGroup = "LoadConfigByDllGroup.json";

            directoryTempNameByPack = "DirectoryTempByPack";
        }

        string INameConfig.FileNameByVersionConfig() => fileNameByVersionConfig;
        string INameConfig.FileTempNameByVersionConfig() => fileTempNameByVersionConfig;
        
        string INameConfig.DirectoryNameByLocalGroup() => directoryNameByLocalGroup;
        string INameConfig.LoadConfigNameByLocalGroup() => loadConfigNameByLocalGroup;
        
        string INameConfig.DirectoryNameByRemoteGroup() => directoryNameByRemoteGroup;
        string INameConfig.DownloadConfigNameByRemoteGroup() => downloadConfigNameByRemoteGroup;
        string INameConfig.DownloadConfigTempNameByRemoteGroup() => downloadConfigTempNameByRemoteGroup;
        string INameConfig.LoadConfigNameByRemoteGroup() => loadConfigNameByRemoteGroup;
        
        string INameConfig.DirectoryNameByDllGroup() => directoryNameByDllGroup;
        string INameConfig.DllsBundleNameByDllGroup() => dllsBundleNameByDllGroup;
        string INameConfig.DownloadConfigNameByDllGroup() => downloadConfigNameByDllGroup;
        string INameConfig.DownloadConfigTempNameByDllGroup() => downloadConfigTempNameByDllGroup;
        string INameConfig.LoadConfigNameByDllGroup() => loadConfigNameByDllGroup;

        string INameConfig.DirectoryTempNameByPack() => directoryTempNameByPack;

    }
}
