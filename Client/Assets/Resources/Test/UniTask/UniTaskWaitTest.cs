using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskWaitTest : MonoBehaviour
    {
        [Header("UniTask 延时")] [SerializeField]
        private PlayerLoopTiming testYieldTiming = PlayerLoopTiming.PreUpdate;

        [SerializeField] private Button btnDelay;
        [SerializeField] private Button btnDelayFrame;
        [SerializeField] private Button btnYield;
        [SerializeField] private Button btnNextFrame;
        [SerializeField] private Button btnEndOfFrame;

        private List<PlayerLoopSystem.UpdateFunction> _injectUpdateFunctions = new();
        private bool _showUpdateLog = false;

        private void Awake()
        {

            btnDelay.onClick.AddListener(OnBtnDelayClick);
            btnDelayFrame.onClick.AddListener(OnBtnDelayFrameClick);
            btnYield.onClick.AddListener(OnBtnYieldClick);
            btnNextFrame.onClick.AddListener(OnBtnNextFrameClick);
            btnEndOfFrame.onClick.AddListener(OnBtnEndOfFrameClick);

            InjectFunction();
        }

        private void OnDestroy()
        {
            UnInjectFunction();
        }

        private async void OnBtnDelayClick()
        {
            Time.time.Log("执行 Delay 开始，当前时间{0}");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Time.time.Log("执行 Delay 结束，当前时间{0}");
        }

        private async void OnBtnDelayFrameClick()
        {
            Time.frameCount.Log("执行 DelayFrane 开始，当前帧{0}");
            await UniTask.DelayFrame(5);
            Time.frameCount.Log("执行 DelayFrame 结束，当前帧{0}");

        }

        private async void OnBtnYieldClick()
        {
            _showUpdateLog = true;
            $"执行 yield 开始".Log();
            await UniTask.Yield(testYieldTiming);
            $"执行 yield 结束".Log();
            _showUpdateLog = false;
        }

        private async void OnBtnNextFrameClick()
        {
            _showUpdateLog = true;
            $"执行 NextFrame 开始".Log();
            await UniTask.NextFrame();
            $"执行 NextFrame 结束".Log();
            _showUpdateLog = false;
        }

        private async void OnBtnEndOfFrameClick()
        {
            _showUpdateLog = true;
            $"执行 WaitEndOfFrame 开始".Log();
            await UniTask.WaitForEndOfFrame(this);
            $"执行 WaitEndOfFrame 结束".Log();
            _showUpdateLog = false;
        }

        private void InjectFunction()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var subsystems = playerLoop.subSystemList;
            playerLoop.updateDelegate += OnUpdate;
            for (int i = 0; i < subsystems.Length; i++)
            {
                int index = i;
                PlayerLoopSystem.UpdateFunction injectFunction = () =>
                {
                    if (!_showUpdateLog) return;
                    $"执行子系统{_showUpdateLog} {subsystems[index]} 当前帧{Time.frameCount}".Log();
                };
                _injectUpdateFunctions.Add(injectFunction);
                subsystems[i].updateDelegate += injectFunction;
            }

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private void UnInjectFunction()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            playerLoop.updateDelegate -= OnUpdate;
            var subsystems = playerLoop.subSystemList;
            for (int i = 0; i < subsystems.Length; i++)
            {
                subsystems[i].updateDelegate -= _injectUpdateFunctions[i];
            }

            PlayerLoop.SetPlayerLoop(playerLoop);
            _injectUpdateFunctions.Clear();
        }

        private void OnUpdate()
        {
            $"当前帧{Time.frameCount}".Log();
        }
    }
}
