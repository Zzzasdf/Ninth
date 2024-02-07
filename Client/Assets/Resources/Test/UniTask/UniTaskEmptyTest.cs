using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskEmptyTest : MonoBehaviour
    {
        [Header("空载测试------")]
        [SerializeField] private int LoopTimes = 100;
        [SerializeField] private Button btnCoroutinueRun;
        [SerializeField] private Button btnUniTaskRun;

        private void Awake()
        {
            btnCoroutinueRun.onClick.AddListener(OnBtnCoroutinueClick);
            btnUniTaskRun.onClick.AddListener(OnBtnUniTaskClick);
        }
    
        private void OnBtnCoroutinueClick()
        {
            StartCoroutine(CoroutineTest(LoopTimes));
        
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
                $"协程耗时测试：{loopTimes}次：耗时{elasedTime * 1000:F6}毫秒".Log();
            }
            IEnumerator EmptyCoroutinue()
            {
                yield return null;
            }
        }
    
        private void OnBtnUniTaskClick()
        {
            UniTaskTest(LoopTimes);
        
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
                $"UniTask耗时测试：{loopTimes}次：耗时{elasedTime * 1000:F6}毫秒".Log();
            }
            async UniTask EmptyUniTask()
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
    }
}