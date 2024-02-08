using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class CallbackSample : MonoBehaviour
    {
        [SerializeField] private Button CallbackButton;
        [SerializeField] private GameObject Target;
        private const float G = 9.8f;
        [SerializeField] private float FallTime = 0.5f;

        private void Start()
        {
            CallbackButton.onClick.AddListener(UniTask.UnityAction(OnClickCallback));
        }

        private async UniTask FallTarget(Transform targetTrans, float fallTime, Action onHalf,
            UniTaskCompletionSource source)
        {
            float startTime = Time.time;

            Vector3 startPosition = targetTrans.position;
            float lastElapsedTime = 0;
            while (Time.time - startTime <= fallTime)
            {
                float elapsedTime = Mathf.Min(Time.time - startTime, fallTime);
                if (lastElapsedTime < fallTime * 0.5f && elapsedTime >= fallTime * 0.5f)
                {
                    onHalf?.Invoke();
                    source.TrySetResult();
                    // 失败
                    // source.TrySetException(new SystemException());
                    // 取消
                    // source.TrySetCanceled(someToken);
                    
                    // 泛型类 UniTaskCompletionSource<T> SetResult 是 T 类型，返回 UniTask<T>
                }
                lastElapsedTime = elapsedTime;
                float fallY = 0 + 0.5f * G * elapsedTime * elapsedTime;
                targetTrans.position = startPosition + Vector3.down * fallY;
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
        }

        private async UniTaskVoid OnClickCallback()
        {
            float time = Time.time;
            UniTaskCompletionSource source = new UniTaskCompletionSource();
            FallTarget(Target.transform, FallTime, OnTargetHalf, source).Forget();
            await source.Task; // UniTaskCompletionSource 产生的 UniTask 是可以复用的
            $"当前缩放{Target.transform.localScale} 耗时 {Time.time - time}秒".Log();
        }

        private void OnTargetHalf()
        {
            Target.transform.localScale *= 1.5f;
        }
    }
}