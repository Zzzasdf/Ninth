using UnityEngine;

namespace Ninth
{
    [CreateAssetMenu(fileName = "NameConfigSO", menuName = "Config/NameConfigSO")]
    public sealed class NameConfig: ScriptableObject
    {
        /// <summary>
        /// 版本配置文件
        /// </summary>
        public string VersionConfigName;

        /// <summary>
        /// 临时版本配置文件
        /// </summary>
        public string TempVersionConfigName;

        /// <summary>
        /// 本地的文件夹名
        /// </summary>
        public string LocalDirectory;

        /// <summary>
        /// 本地的加载配置
        /// </summary>
        public string LoadConfigNameInLocal;

        /// <summary>
        /// 远端的热更文件夹
        /// </summary>
        public string RemoteDirectory;

        /// <summary>
        /// 在远端的热更下载配置名
        /// </summary>
        public string DownloadConfigNameInRemote;

        /// <summary>
        /// 在远端的热更下载临时配置名
        /// </summary>
        public string TempDownloadConfigNameInRemote;

        /// <summary>
        /// 在远端的热更加载配置名
        /// </summary>
        public string LoadConfigNameInRemote;

        /// <summary>
        /// Dll文件夹
        /// </summary>
        public string DllDirectory;

        /// <summary>
        /// Dll程序集集合的Bundle名
        /// </summary>
        public string DllsBundleName;

        /// <summary>
        /// Dll的下载配置名
        /// </summary>
        public string DownloadConfigNameInDll;

        /// <summary>
        /// Dll的下载临时配置名
        /// </summary>
        public string TempDownloadConfigNameInDll;

        /// <summary>
        /// Dll的加载配置名
        /// </summary>
        public string LoadConfigNameInDll;

        /// <summary>
        /// 打包的临时目录
        /// </summary>
        public string PackTempDirectory;

        private NameConfig()
        {
            VersionConfigName = "VersionConfig.json";
            TempVersionConfigName = "TempVersionConfig.json";

            // 本地
            LocalDirectory = "Local";
            LoadConfigNameInLocal = "LoadLocal.json";

            // 远端
            RemoteDirectory = "Remote";
            DownloadConfigNameInRemote = "DownloadRemote.json";
            TempDownloadConfigNameInRemote = "TempDownloadRemote.json";
            LoadConfigNameInRemote = "LoadRemote.json";

            // Dll
            DllDirectory = "Dll";
            DllsBundleName = "Assemblys";
            DownloadConfigNameInDll = "DownloadDll.json";
            TempDownloadConfigNameInDll = "TempDownloadDll.json";
            LoadConfigNameInDll = "LoadDll.json";

            // 打包的临时目录
            PackTempDirectory = "TempPackDirectory";
        }
    }
}
