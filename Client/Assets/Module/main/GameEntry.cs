using UnityEngine;

namespace Ninth
{
    [DisallowMultipleComponent]
    public sealed partial class GameEntry : MonoBehaviour
    {
        public static ConfigCore Config; // 配置
        public static DownloadCore Download { get; private set; } // 下载
        public static JsonCore Json { get; private set; } // Json
        public static ProcedureCore Procedure { get; private set; } // 流程

        private void Awake()
        {
            Config = new ConfigCore();
            Download = new DownloadCore();
            Json = new JsonCore(Config.Encoding);
            Procedure = new ProcedureCore(Config.AssetConfig, Config.PathConfig, Download);
        }

        void Start()
        {
            Procedure.Start();
        }
    }
}