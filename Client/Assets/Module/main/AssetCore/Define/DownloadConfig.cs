using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
{
    public class DownloadConfig: IJson
    {
        public Dictionary<string, BundleInfo> BundleInfos;

        public DownloadConfig()
        {
            BundleInfos = new Dictionary<string, BundleInfo>();
        }

        public static List<BundleInfo>? UpdateCompare(DownloadConfig? server, DownloadConfig? persistentData, CancellationToken cancellationToken = default)
        {
            if (server == null)
            {
                "无法加载到 Server 下的下载文件".FrameError();
                return null;
            }
            if (persistentData == null)
            {
                // 全部下载
                return server.BundleInfos.Values.ToList();
            }
            
            var bundleInfos = from item in server.BundleInfos 
                                                    where !persistentData.BundleInfos.ContainsKey(item.Key) 
                                                    select item.Value;
            return bundleInfos.ToList();
        }
    }
}