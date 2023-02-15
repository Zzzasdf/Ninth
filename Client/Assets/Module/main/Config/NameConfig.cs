namespace Ninth
{
    public class NameConfig
    {
        /// <summary>
        /// 版本配置文件
        /// </summary>
        public static string VersionConfigName { get; private set; }

        /// <summary>
        /// 临时版本配置文件
        /// </summary>
        public static string TempVersionConfigName { get; private set; }

        /// <summary>
        /// 本地的文件夹名
        /// </summary>
        public static string LocalDirectory { get; private set; }

        /// <summary>
        /// 本地的加载配置
        /// </summary>
        public static string LoadConfigNameInLocal { get; private set; }

        /// <summary>
        /// 远端的热更文件夹
        /// </summary>
        public static string RemoteDirectory { get; private set; }

        /// <summary>
        /// 在远端的热更下载配置名
        /// </summary>
        public static string DownloadConfigNameInRemote { get; private set; }

        /// <summary>
        /// 在远端的热更下载临时配置名
        /// </summary>
        public static string TempDownloadConfigNameInRemote { get; private set; }

        /// <summary>
        /// 在远端的热更加载配置名
        /// </summary>
        public static string LoadConfigNameInRemote { get; private set; }

        /// <summary>
        /// Dll文件夹
        /// </summary>
        public static string DllDirectory { get; private set; }

        /// <summary>
        /// Dll程序集集合的Bundle名
        /// </summary>
        public static string DllsBundleName { get; private set; }

        /// <summary>
        /// Dll的下载配置名
        /// </summary>
        public static string DownloadConfigNameInDll { get; private set; }

        /// <summary>
        /// Dll的下载临时配置名
        /// </summary>
        public static string TempDownloadConfigNameInDll { get; private set; }

        /// <summary>
        /// Dll的加载配置名
        /// </summary>
        public static string LoadConfigNameInDll { get; private set; }

        /// <summary>
        /// 打包的临时目录
        /// </summary>
        public static string PackTempDirectory { get; private set; }

        static NameConfig()
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
