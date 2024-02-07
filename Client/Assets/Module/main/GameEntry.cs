using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        async void Awake()
        {
 
            // _Json = new JsonCore();
        }

        void Start()
        {
            // _Procedure.Start();
        }
    }
}