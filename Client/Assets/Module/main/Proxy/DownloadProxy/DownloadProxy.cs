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

        async UniTask<bool> IDownloadProxy.DownloadAsync(string? srcPath, string? dstPath, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(dstPath))
            {
                $"下载的源路径或目标路径为空，源路径：{srcPath}, 目标路径: {dstPath}".Error();
                return false;
            }
            var request = UnityWebRequest.Get(srcPath);
            request.downloadHandler = new DownloadHandlerFile(dstPath);
            await request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error) == false)
            {
                $"下载错误, 远端路径: {srcPath}, 本地路径: {dstPath}, 错误日志: {request.error}".Error();
                return false;
            }
            return true;
        }
    }
}