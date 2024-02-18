using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks.Linq;

namespace Ninth
{
    public class AssetDownloadBox: IAssetDownloadBox
    {
        private readonly IPlayerPrefsStringProxy playerPrefsStringProxy;
        private readonly IPlayerPrefsIntProxy playerPrefsIntProxy;
        private readonly IPathProxy pathProxy;
        private readonly IDownloadProxy downloadProxy;
        
        private readonly string AssetPath = "LoadView/Prefabs/MessageBox";
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
            
            var node = Object.Instantiate(Resources.Load<GameObject>("LoadView/Prefabs/MessageBox"));
            var btnDic = node.transform.GetComponentsInChildren<Button>().ToDictionary(value => value.name, value => value);
            var txtDic = node.transform.GetComponentsInChildren<Text>().ToDictionary(value => value.name, value => value);
            
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
                downloadedCount.Value = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos) ?? 0 - skipCount;
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
            var startPos = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos);
            if (startPos == null)
            {
                "无法获取到断点续传的起始位置".FrameError();
                return false;
            }
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
            
            // 初始化
            this.version = version;
            bundleInfosGroup = downloadBundleInfos;
            skipCount = startPos.Value; // 记录进入界面时断点的位置
            
            // 结果
            await UniTask.WaitUntil(() => completeStatus.HasValue, cancellationToken: cancellationToken);
            downloadBundleCancellationToken?.Dispose();
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
            var startPos = playerPrefsIntProxy.Get(PLAYERPREFS_INT.DownloadBundleStartPos) ?? 0;
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
            var (srcPath, tempPathOrNull) = pathProxy.Get(bundlePath, version, bundleName);
            if (tempPathOrNull == null)
            {
                $"无下载的本地目标路径 源路径：{srcPath}".Error();
                return false;
            }
            var tempPath = tempPathOrNull.Value;
            var dstPath = pathProxy.Get(tempPath, bundleName);
            return await downloadProxy.DownloadAsync(srcPath, dstPath, cancellationToken);
        }
    }
}