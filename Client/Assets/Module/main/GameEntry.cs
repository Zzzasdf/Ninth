using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

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
            // _Json = new JsonCore();
            btnCoroutinueRun.onClick.AddListener(OnBtnCoroutinue);
            btnUniTaskRun.onClick.AddListener(OnBtnUniTask);
        }

        void Start()
        {
            // _Procedure.Start();
        }

        // 空载性能测试
        [Header("空载测试------")]
        [SerializeField] private int LoopTimes = 100;
        [SerializeField] private Button btnCoroutinueRun;
        [SerializeField] private Button btnUniTaskRun;
        
        private void OnBtnUniTask()
        {
            UniTaskTest(LoopTimes);
        }

        private void OnBtnCoroutinue()
        {
            StartCoroutine(CoroutineTest((LoopTimes))); ;
        }
        
        // 协程
        IEnumerator CoroutineTest(int loopTimes)
        {
            float elasedTime = 0;
            for (int i = 0; i < loopTimes; i++)
            {
                float time = Time.realtimeSinceStartup;
                var coroutine = StartCoroutine(EmptyCoroutinue());
                elasedTime += (Time.realtimeSinceStartup - time);
                yield return coroutine;
            }
            Debug.Log($"协程耗时测试：{loopTimes}次：耗时{elasedTime * 1000:F6}毫秒");
        }
        IEnumerator EmptyCoroutinue()
        {
            yield return null;
        }
            
        // UniTask
        async void UniTaskTest(int loopTimes)
        {
            float elasedTime = 0;
            for (int i = 0; i < loopTimes; i++)
            {
                float time = Time.realtimeSinceStartup;
                var uniTask = EmptyUniTask();
                elasedTime += (Time.realtimeSinceStartup - time);
                await uniTask;
            }
            Debug.Log($"UniTask耗时测试：{loopTimes}次：耗时{elasedTime * 1000:F6}毫秒");
        }

        async UniTask EmptyUniTask()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}