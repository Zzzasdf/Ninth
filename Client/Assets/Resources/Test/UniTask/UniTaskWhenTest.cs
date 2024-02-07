using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskWhenTest : MonoBehaviour
    { 
        [SerializeField] private Runner FirstRunner;
        [SerializeField] private Runner SecondRuner;
        
        [SerializeField] private Button FirstRunButton;
        [SerializeField] private Button SecondRunButton;

        [SerializeField] private Button FirstCannelButton;
        [SerializeField] private Button SecondCancelButton;
        
        [SerializeField] private Button WhenAllButton;
        [SerializeField] private Button WhenAnyButton;
        [SerializeField] private Button ResetButton;

        [SerializeField] private float TotalDistance = 15;

        private CancellationTokenSource _firstCancelToken;
        private CancellationTokenSource _secondCancelToken;
        private CancellationTokenSource _linkedCancelToken;
        
        [SerializeField] private Text WinnerText;
        [SerializeField] private Text CompleteText;
        [SerializeField] private Text FirstText;
        [SerializeField] private Text SecondText;
        
        private void Start()
        {
            FirstRunButton.onClick.AddListener(OnClickFirstRun);
            SecondRunButton.onClick.AddListener(OnClickSecondRun);

            FirstCannelButton.onClick.AddListener(OnClickFirstCancel);
            SecondCancelButton.onClick.AddListener(OnClickSecondCancel);

            WhenAllButton.onClick.AddListener(OnClickWhenAll);
            WhenAnyButton.onClick.AddListener(OnClickWhenAny);
            ResetButton.onClick.AddListener(OnClickReset);

            _firstCancelToken = new CancellationTokenSource();
            // 注意这里可以直接先行设置多久以后取消
            // _firstCancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(1.5f));
            _secondCancelToken = new CancellationTokenSource();
            _linkedCancelToken = CancellationTokenSource.CreateLinkedTokenSource(_firstCancelToken.Token, _secondCancelToken.Token);
        }

        private void OnClickReset()
        {
            FirstRunner.Reset();
            SecondRuner.Reset();
            CompleteText.text = "";
            WinnerText.text = "";
        }

        private async UniTask RunSomeOne(Runner runner, CancellationToken cancellationToken)
        {
            runner.Reset();
            float totalTime = TotalDistance / runner.Speed;
            float timeElapsed = 0;
            while (timeElapsed <= totalTime)
            {
                timeElapsed += Time.deltaTime;
                await UniTask.NextFrame(cancellationToken);
                float runDistance = Mathf.Min(timeElapsed, totalTime) * runner.Speed;
                runner.Target.position = runner.StartPos + Vector3.right * runDistance;
            }
            runner.ReachGoal = true;
        }

        private void OnClickFirstCancel()
        {
            _firstCancelToken.Cancel();
            _firstCancelToken.Dispose();
            _firstCancelToken = new CancellationTokenSource();
            _linkedCancelToken =
                CancellationTokenSource.CreateLinkedTokenSource(_firstCancelToken.Token, _secondCancelToken.Token);
        }

        private void OnClickSecondCancel()
        {
            _secondCancelToken.Cancel();
            _secondCancelToken.Dispose();
            _secondCancelToken = new CancellationTokenSource();
            _linkedCancelToken =
                CancellationTokenSource.CreateLinkedTokenSource(_firstCancelToken.Token, _secondCancelToken.Token);
        }

        private void OnDestroy()
        {
            _firstCancelToken.Dispose();
            _secondCancelToken.Dispose();
            _linkedCancelToken.Dispose();
        }

        private async void OnClickFirstRun()
        {
            try
            {
                await RunSomeOne(FirstRunner, _firstCancelToken.Token);
            }
            catch (OperationCanceledException e)
            {
                FirstText.text = "1号跑已经被取消";
            }
        }

        private async void OnClickSecondRun()
        {
            bool cancelled = await RunSomeOne(SecondRuner, _linkedCancelToken.Token).SuppressCancellationThrow();
            if (cancelled)
            {
                SecondText.text = "2号跑已经被取消";
            }
        }

        private async void OnClickWhenAll()
        {
            var firstRunnerReach = UniTask.WaitUntil(() => FirstRunner.ReachGoal);
            // 注意还有个 WaitUnitValueChanged, 也很有用！
            var secondRunerReach = UniTask.WaitUntil(() => SecondRuner.ReachGoal);
            await UniTask.WhenAll(firstRunnerReach, secondRunerReach);
            // 注意，whenAll 可以用于平行执行多个资源的读取，非常有用!
            // var (a, b, c) = await UniTask.WhenAll(
            // LoadAsSprite("foo"),
            // LoadAsSprite("bar"),
            // LoadAsSprite("baz"));
            CompleteText.text = "双方都抵达了终点，比赛结束";
        }

        private async void OnClickWhenAny()
        {
            var firstRunnerReach = UniTask.WaitUntil(() => FirstRunner.ReachGoal);
            var secondRunnerReach = UniTask.WaitUntil(() => SecondRuner.ReachGoal);
            await UniTask.WhenAny(firstRunnerReach, secondRunnerReach);
            string winner = FirstRunner.ReachGoal ? "蓝色小球" : "黄色小球";
            WinnerText.text = $"{winner}率先抵达了终点，获得了胜利";
        }
    }
}
