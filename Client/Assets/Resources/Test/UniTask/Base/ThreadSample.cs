using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class ThreadSample : MonoBehaviour
    {
        [SerializeField] private Button StandardRun;
        [SerializeField] private Button YieldRun;

        [SerializeField] private Text Text;

        private void Start()
        {
            StandardRun.onClick.AddListener(UniTask.UnityAction(OnClickStandardRun));
            YieldRun.onClick.AddListener(UniTask.UnityAction(OnClickYieldRun));
        }

        private async UniTaskVoid OnClickStandardRun()
        {
            int result = 0;
            await UniTask.RunOnThreadPool(() => { result = 1; });
            await UniTask.SwitchToMainThread();
            Text.text = $"计算结束，当前结果是{result}";
        }
        
        private async UniTaskVoid OnClickYieldRun()
        {
            string fileName = Application.dataPath + "/Resources/Test/UniTask/test.txt";
            await UniTask.SwitchToThreadPool();
            string fileContent = await File.ReadAllTextAsync(fileName);
            await UniTask.Yield(PlayerLoopTiming.Update);
            Text.text = fileContent;
        }
    }
}
