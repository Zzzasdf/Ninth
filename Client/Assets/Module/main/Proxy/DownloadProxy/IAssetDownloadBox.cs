using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ninth
{
    public interface IAssetDownloadBox
    {
        UniTask<bool> PopUpAsync(string version, CancellationToken cancellationToken = default, params (ASSET_SERVER_BUNDLE_PATH bundlePath, List<BundleInfo>? bundleInfos)[] downloadBundleInfos);
    }
}