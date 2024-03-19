using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace Ninth
{
    public class DownloadProxy: IDownloadProxy
    {
        [Inject]
        public DownloadProxy()
        {
            
        }

        async UniTask<bool> IDownloadProxy.DownloadAsync(string serverPath, string cachePath, CancellationToken cancellationToken)
        {
            var request = UnityWebRequest.Get(serverPath);
            request.downloadHandler = new DownloadHandlerFile(cachePath);
            await request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error) == false)
            {
                $"下载错误, 远端路径: {serverPath}, 本地路径: {cachePath}, 错误日志: {request.error}".Error();
                return false;
            }
            else
            {
                $"下载成功, 远端路径: {serverPath}, 本地路径: {cachePath}".Log();
            }
            return true;
        }
    }
}