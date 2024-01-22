using UnityEngine;
using System;

namespace Ninth
{
    [DisallowMultipleComponent]
    public sealed partial class GameEntry: MonoBehaviour
    {
        [SerializeField] private ConfigCore _Config; // 配置
        [SerializeField] private DownloadCore _Download; // 下载
        [SerializeField] private JsonCore _Json; // Json
        [SerializeField] private ProcedureCore _Procedure; // 流程

        public ConfigCore Config => _Config;
        public DownloadCore Download => _Download;
        public JsonCore Json => _Json;
        public ProcedureCore Procedure => _Procedure;

        void Awake()
        {
            _Json = new JsonCore();
        }

        void Start()
        {
            _Procedure.Start();
        }
    }
}