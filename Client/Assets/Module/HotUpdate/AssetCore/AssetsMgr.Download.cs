using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.Networking;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr
    {
        public async UniTask<string> GetRequest(string url, float timeout)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));

            var (cancelOrFailed, result) = await UnityWebRequest.Get(url)
                .SendWebRequest() // 发送请求
                .WithCancellation(cts.Token) // 注册取消
                .SuppressCancellationThrow(); // 忽略异常

            if(!cancelOrFailed)
            {
                return result.downloadHandler.text;
            }
            return "取消或超时";
        }

        public async void TestRequest()
        {
            string text = await GetRequest("https://fanyi.baidu.com/translate?aldtype=16047&query=Unloaded&keyfrom=baidu&smartresult=dict&lang=auto2zh#en/zh/pending", 5);
            text.Substring(0, 1000).Log();
        }
    }
}