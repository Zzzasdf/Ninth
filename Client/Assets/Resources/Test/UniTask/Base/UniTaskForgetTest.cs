using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskForgetTest : MonoBehaviour
    {
        [SerializeField] private Button StartButton;
        [SerializeField] private GameObject FirstTarget;
        [SerializeField] private GameObject SecondTarget;

        private const float G = 9.8f;
        [SerializeField] private float FirstFallTime = 2f;
        [SerializeField] private float SecondFallTime = 2f;

        private void Start()
        {
            StartButton.onClick.AddListener(OnClickStart);            
        }

        private async UniTaskVoid FallTarget(Transform targetTrans, float fallTime)
        {
            float startTime = Time.time;
            Vector3 startPosition = targetTrans.position;
            while (Time.time - startTime <= fallTime)
            {
                float elapsedTime = Mathf.Min(Time.time - startTime, fallTime);
                float fallY = 0 + 0.5f * G * elapsedTime * elapsedTime;
                targetTrans.position = startPosition + Vector3.down * fallY;
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
        }

        private void OnClickStart()
        {
            FallTarget(FirstTarget.transform, FirstFallTime).Forget();
            FallTarget(SecondTarget.transform, SecondFallTime).Forget();
        }
    }
}