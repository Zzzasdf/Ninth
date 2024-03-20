using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Object = UnityEngine.Object;

namespace Ninth
{
    public class AssetDownloadBox: IAssetDownloadBox
    {
        private readonly IPlayerPrefsStringProxy playerPrefsStringProxy;
        private readonly IPlayerPrefsIntProxy playerPrefsIntProxy;
        private readonly IPathProxy pathProxy;
        private readonly IDownloadProxy downloadProxy;
        
        private readonly string AssetPath = "LoadView/Prefabs/MessageBox";
        private GameObject gameObject;
        private readonly Text txtMessage;
        private readonly Button btnFirst;
        private bool btnFirstClicked;

        private readonly Button btnSecond;

        private readonly Button btnThird;

        private string version;
        private (ASSET_SERVER_BUNDLE_PATH bundlePath, List<BundleInfo>? bundleInfos)[] bundleInfosGroup;
        private readonly AsyncReactiveProperty<int> downloadedCount;
        private CancellationTokenSource? downloadBundleCancellationToken;
        private int skipCount;
        private bool? completeStatus;

        [Inject]
        public AssetDownloadBox(IPlayerPrefsStringProxy playerPrefsStringProxy, IPlayerPrefsIntProxy playerPrefsIntProxy, IPathProxy pathProxy, IDownloadProxy downloadProxy)
        {
            this.playerPrefsStringProxy = playerPrefsStringProxy;
            this.playerPrefsIntProxy = playerPrefsIntProxy;
            this.pathProxy = pathProxy;
            this.downloadProxy = downloadProxy;
            
            gameObject = Object.Instantiate(Resources.Load<GameObject>(AssetPath));
            var btnDic = gameObject.transform.GetComponentsInChildren<Button>().ToDictionary(value => value.name, value => value);
            var txtDic = gameObject.transform.GetComponentsInChildren<Text>().ToDictionary(value => value.name, value => value);
            
            txtMessage = txtDic["txtMessage"];
            btnFirst = btnDic["btnFirst"];
            var txtFirst = txtDic["txtFirst"];
            txtFirst.text = "下载";
            btnFirstClicked = false;
            
            btnSecond = btnDic["btnSecond"];
            var txtSecond = txtDic["txtSecond"];
            txtSecond.text = "退出游戏";

            btnThird = btnDic["btnThird"];
            var txtThird = txtDic["txtThird"];
            txtThird.text = "取消";
            
            btnFirst.gameObject.SetActive(true);
            btnSecond.gameObject.SetActive(true);
            btnThird.gameObject.SetActive(false);

            btnFirst.onClick.AddListener(UniTask.UnityAction(BtnFirstClick));
            btnSecond.onClick.AddListener(BtnSecondClick);
            btnThird.onClick.AddListener(BtnThirdClick);
            
            downloadedCount = new AsyncReactiveProperty<int>(0);
        }

        private async UniTaskVoid BtnFirstClick()
        {
            if (!btnFirstClicked)
            {
                btnFirstClicked = true;
                btnFirst.gameObject.SetActive(false);
                btnSecond.gameObject.SetActive(false);
                btnThird.gameObject.SetActive(true);
                downloadBundleCancellationToken = new CancellationTokenSource();
                downloadedCount.Value = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos);
                var cancelled = await DownloadOperateAsync(downloadBundleCancellationToken.Token).SuppressCancellationThrow();
                if (cancelled)
                {
                    btnFirstClicked = false;
                    btnFirst.gameObject.SetActive(true);
                    btnSecond.gameObject.SetActive(true);
                    btnThird.gameObject.SetActive(false);
                    var unDownloadSize = GetUnDownloadSize();
                    txtMessage.text = $"发现新的版本, 需要下载的资源大小: {SizeToString(unDownloadSize)}";
                    return;
                }
                completeStatus = true;
            }
        }
        
        private void BtnSecondClick()
        {
            completeStatus = false;
        }
        
        private void BtnThirdClick()
        {
            downloadBundleCancellationToken?.Cancel();
            downloadBundleCancellationToken?.Dispose();
        }
        
        async UniTask<bool> IAssetDownloadBox.PopUpAsync(string version, CancellationToken cancellationToken, params (ASSET_SERVER_BUNDLE_PATH bundlePath, List<BundleInfo>? bundleInfos)[] downloadBundleInfos)
        {
            var isDownload = false;
            foreach (var tuple in downloadBundleInfos)
            {
                if (tuple.bundleInfos == null || tuple.bundleInfos.Count == 0)
                {
                    continue;
                }
                isDownload = true;
            }
            if (!isDownload)
            {
                "没有需要下载的 bundle".FrameError();
                return false;
            }
            if (version != playerPrefsStringProxy.Get(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion))
            {
                playerPrefsStringProxy.Set(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, version);
                playerPrefsIntProxy.Set(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            }
            // 初始化
            this.version = version;
            bundleInfosGroup = downloadBundleInfos;
            skipCount = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos); // 记录进入界面时断点的位置
            var unDownloadSize = GetUnDownloadSize();
            txtMessage.text = $"发现新的版本, 需要下载的资源大小: {SizeToString(unDownloadSize)}";
            
            // 订阅
            downloadedCount.Subscribe(value =>
            {
                if (downloadBundleCancellationToken == null || downloadBundleCancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var size = GetDownloadedSize(value);
                var totalSize = GetTotalDownloadSize();
                var totalBundleCount = GetTotalBundleCount();
                txtMessage.text = $"当前的下载进度{(float)size / totalSize}%({value}/{totalBundleCount})";
            });
            
            // 结果
            await UniTask.WaitUntil(() => completeStatus.HasValue, cancellationToken: cancellationToken);
            downloadBundleCancellationToken?.Dispose();
            Object.Destroy(gameObject);
            return completeStatus.HasValue && completeStatus.Value;
        }

        private async UniTask DownloadOperateAsync(CancellationToken cancellationToken = default)
        {
            foreach (var bundleInfos in bundleInfosGroup)
            {
                if (bundleInfos.bundleInfos == null)
                {
                    continue;
                }
                foreach (var bundleInfo in bundleInfos.bundleInfos)
                {
                    var (isCancelled, isSuccess) = await DownloadBundle(bundleInfos.bundlePath, version, bundleInfo.BundleName, cancellationToken).SuppressCancellationThrow();
                    if (isCancelled || !isSuccess)
                    {
                        continue;
                    }
                    var startPos = playerPrefsStringProxy.Get(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion);
                    if (startPos == null)
                    {
                        continue;
                    }
                    playerPrefsStringProxy.Set(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, version);
                    downloadedCount.Value++;
                }
            }
        }


        private string SizeToString(int size)
        {
            var sizeStr = "";
            if (size >= 1024 * 1024)
            {
                long m = size / (1024 * 1024);
                size = size % (1024 * 1024);
                sizeStr += $"{m}[M]";
            }
            if (size >= 1024)
            {
                long k = size / 1024;
                size = size % 1024;
                sizeStr += $"{k}[K]";
            }
            long b = size;
            sizeStr += $"{b}[B]";
            return sizeStr;
        }

        private int GetDownloadedSize(int downloadedCount)
        {
            var size = 0;
            var index = 0;
            for (var i = 0; i < bundleInfosGroup.Length; i++)
            {
                var bundleInfos = bundleInfosGroup[i].bundleInfos;
                for (var j = 0; j < bundleInfos?.Count; j++)
                {
                    if (index == skipCount + downloadedCount)
                    {
                        return size;
                    }
                    if (index >= skipCount)
                    {
                        size = bundleInfos[j].Size;
                    }
                    index++;
                }
            }
            return size;
        }

        private int GetUnDownloadSize()
        {
            var size = 0;
            var index = 0;
            var startPos = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos);
            for (var i = 0; i < bundleInfosGroup.Length; i++)
            {
                var bundleInfos = bundleInfosGroup[i].bundleInfos;
                for (var j = 0; j < bundleInfos?.Count; j++)
                {
                    if (index >= startPos)
                    {
                        size = bundleInfos[j].Size;
                    }
                    index++;
                }
            }
            return size;
        }

        private int GetTotalDownloadSize()
        {
            var size = 0;
            var index = 0;
            for (var i = 0; i < bundleInfosGroup.Length; i++)
            {
                var bundleInfos = bundleInfosGroup[i].bundleInfos;
                for (var j = 0; j < bundleInfos?.Count; j++)
                {
                    if (index >= skipCount)
                    {
                        size = bundleInfos[j].Size;
                    }
                    index++;
                }
            }
            return size;
        }

        private int GetTotalBundleCount()
        {
            var count = 0;
            foreach (var item in bundleInfosGroup)
            {
                if (item.bundleInfos == null)
                {
                    continue;
                }
                foreach (var item2 in item.bundleInfos)
                {
                    count++;
                }
            }
            return count;
        }
        
        private async UniTask<bool> DownloadBundle(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName, CancellationToken cancellationToken)
        {
            var (serverPath, cachePath) = pathProxy.Get(bundlePath, version, bundleName);
            var dstPath = pathProxy.Get(cachePath, bundleName);
            return await downloadProxy.DownloadAsync(serverPath, dstPath, cancellationToken);
        }
    }
}