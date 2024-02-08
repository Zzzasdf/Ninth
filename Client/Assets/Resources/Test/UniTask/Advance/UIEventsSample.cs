using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks.Linq;

namespace UniTaskTutorial.Advance.Scripts
{
    public class UIEventsSample : MonoBehaviour
    {
        [SerializeField] private float DoubleClickCheckTime = 0.5f;
        [SerializeField] private Button DoubleClickButton;
        [SerializeField] private Text DoubleEventText;

        [SerializeField] private Button CoolDownButton;
        [SerializeField] private Button SphereButton;

        [SerializeField] private Text CoolDownEventText;

        [SerializeField] private float CooldownTime = 3f;

        private void Start()
        {
            CheckDoubleClickButton(DoubleClickButton, this.GetCancellationTokenOnDestroy()).Forget();
            CheckSphereClick(SphereButton.GetCancellationTokenOnDestroy()).Forget();
            CheckCooldownClickButton(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid CheckSphereClick(CancellationToken token)
        {
            var asyncEnumerable = SphereButton.OnClickAsAsyncEnumerable();
            await asyncEnumerable.Take(3).ForEachAsync((_, index) =>
            {
                if (token.IsCancellationRequested) return;
                if (index == 0)
                {
                    SphereTweenScale(2, SphereButton.transform.localScale.x, 20, token).Forget();
                }
                else if(index == 1)
                {
                    SphereTweenScale(2, SphereButton.transform.localScale.x, 10, token).Forget();
                }
            }, token);
            GameObject.Destroy(SphereButton.gameObject);
        }

        private async UniTaskVoid SphereTweenScale(float totalTime, float from, float to, CancellationToken token)
        {
            float time = 0;
            while (time < totalTime)
            {
                time += Time.deltaTime;
                transform.localScale = (from + (time / totalTime) * (to - from)) * Vector3.one;
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private async UniTaskVoid CheckCooldownClickButton(CancellationToken token)
        {
            var asyncEnumerable = CoolDownButton.OnClickAsAsyncEnumerable();
            await asyncEnumerable.ForEachAwaitAsync(async (_) => //.Queue().ForEachAwaitAsync(async (_) =>
            {
                CoolDownEventText.text = "被点击了，冷却中...".Log();
                await UniTask.Delay(TimeSpan.FromSeconds(CooldownTime), cancellationToken: token);
                CoolDownEventText.text = "冷却好了，可以点了...".Log();
            }, token);
        }

        private async UniTaskVoid CheckDoubleClickButton(Button button, CancellationToken token)
        {
            while (true)
            {
                var clickAsync = button.OnClickAsync(token);
                await clickAsync;
                DoubleEventText.text = "按钮被第一次点击";
                var secondClickAsync = button.OnClickAsync(token);
                int resultIndex = await UniTask.WhenAny(secondClickAsync,
                    UniTask.Delay(TimeSpan.FromSeconds(DoubleClickCheckTime), cancellationToken: token));
                if (resultIndex == 0)
                {
                    DoubleEventText.text = "按钮被双击了";
                }
                else
                {
                    DoubleEventText.text = "超时，按钮算单次点击";
                }
            }
        }
    }
}
