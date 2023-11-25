using System;
using UnityEngine;

namespace Ninth
{
    [CreateAssetMenu(fileName = "NameConfigSO", menuName = "Config/NameConfigSO")]
    [Serializable]
    public sealed class NameConfig: ScriptableObject
    {
        /// <summary>
        /// 版本配置文件
        /// </summary>
        [SerializeField] private string versionConfigName;
        public string VersionConfigName => versionConfigName;

        /// <summary>
        /// 临时版本配置文件
        /// </summary>
        [SerializeField] private string tempVersionConfigName;
        public string TempVersionConfigName => tempVersionConfigName;

        /// <summary>
        /// 本地的文件夹名
        /// </summary>
        [SerializeField] private string localDirectory;
        public string LocalDirectory => localDirectory;

        /// <summary>
        /// 本地的加载配置
        /// </summary>
        [SerializeField] private string loadConfigNameInLocal;
        public string LoadConfigNameInLocal => loadConfigNameInLocal;

        /// <summary>
        /// 远端的热更文件夹
        /// </summary>
        [SerializeField] private string remoteDirectory;
        public string RemoteDirectory => remoteDirectory;

        /// <summary>
        /// 在远端的热更下载配置名
        /// </summary>
        [SerializeField] private string downloadConfigNameInRemote;
        public string DownloadConfigNameInRemote => downloadConfigNameInRemote;

        /// <summary>
        /// 在远端的热更下载临时配置名
        /// </summary>
        [SerializeField] private string tempDownloadConfigNameInRemote;
        public string TempDownloadConfigNameInRemote => tempDownloadConfigNameInRemote;

        /// <summary>
        /// 在远端的热更加载配置名
        /// </summary>
        [SerializeField] private string loadConfigNameInRemote;
        public string LoadConfigNameInRemote => loadConfigNameInRemote;

        /// <summary>
        /// Dll文件夹
        /// </summary>
        [SerializeField] private string dllDirectory;
        public string DllDirectory => dllDirectory;

        /// <summary>
        /// Dll程序集集合的Bundle名
        /// </summary>
        [SerializeField] private string dllsBundleName;
        public string DllsBundleName => dllsBundleName;

        /// <summary>
        /// Dll的下载配置名
        /// </summary>
        [SerializeField] private string downloadConfigNameInDll;
        public string DownloadConfigNameInDll => downloadConfigNameInDll;

        /// <summary>
        /// Dll的下载临时配置名
        /// </summary>
        [SerializeField] private string tempDownloadConfigNameInDll;
        public string TempDownloadConfigNameInDll => tempDownloadConfigNameInDll;

        /// <summary>
        /// Dll的加载配置名
        /// </summary>
        [SerializeField] private string loadConfigNameInDll;
        public string LoadConfigNameInDll => loadConfigNameInDll;

        /// <summary>
        /// 打包的临时目录
        /// </summary>
        [SerializeField] private string packTempDirectory;
        public string PackTempDirectory => packTempDirectory;

        public NameConfig()
        {
            versionConfigName = "VersionConfig.json";
            tempVersionConfigName = "TempVersionConfig.json";

            // 本地
            localDirectory = "Local";
            loadConfigNameInLocal = "LoadLocal.json";

            // 远端
            remoteDirectory = "Remote";
            downloadConfigNameInRemote = "DownloadRemote.json";
            tempDownloadConfigNameInRemote = "TempDownloadRemote.json";
            loadConfigNameInRemote = "LoadRemote.json";

            // Dll
            dllDirectory = "Dll";
            dllsBundleName = "Assemblys";
            downloadConfigNameInDll = "DownloadDll.json";
            tempDownloadConfigNameInDll = "TempDownloadDll.json";
            loadConfigNameInDll = "LoadDll.json";

            // 打包的临时目录
            packTempDirectory = "TempPackDirectory";
        }
    }
}
