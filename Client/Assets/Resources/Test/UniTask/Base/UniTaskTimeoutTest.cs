using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class UniTaskTimeoutTest : MonoBehaviour
    {
        [SerializeField] private Button testButton;
        [SerializeField] private string SearchWord = "Unity";
        [SerializeField] private string[] SearchURLs = new string[]
        {
            "https://www.baidu.com/s?wd=",
            "https://www.bing.com/search?q=",
            "https://www.google.com/search?q="
        };

        [SerializeField] private Text[] Texts;

        private void Start()
        {
            testButton.onClick.AddListener(UniTask.UnityAction(OnClickTest));
        }

        private async UniTask<string> GetRequest(string url, float timeout)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));

            var (cancelOrFailed, result) = await UnityWebRequest.Get(url).SendWebRequest().WithCancellation(cts.Token).SuppressCancellationThrow();
            if (!cancelOrFailed)
            {
                return result.downloadHandler.text.Substring(0, 100);
            }
            return "取消或超时";
        }

        private async UniTaskVoid OnClickTest()
        {
            UniTask<string>[] waitTasks = new UniTask<string>[SearchURLs.Length];
            for (int i = 0; i < SearchURLs.Length; i++)
            {
                waitTasks[i] = GetRequest(SearchURLs[i], 2f);
            }

            var tasks = await UniTask.WhenAll(waitTasks);
            for (int i = 0; i < tasks.Length; i++)
            {
                Texts[i].text = tasks[i];
            }
        }
    }
}
