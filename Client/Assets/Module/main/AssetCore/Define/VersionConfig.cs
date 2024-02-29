using System.Threading;
using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
{
    public class VersionConfig: IJson
    {
        public string DisplayVersion { get; set; }
        public int FrameVersion { get; set; }
        public int HotUpdateVersion { get; set; }
        public int IterateVersion { get; set; }

        public string BuiltIn => $"{FrameVersion}.{HotUpdateVersion}.{IterateVersion}";

        public static UniTask<bool> UpdateCompare(VersionConfig? server, VersionConfig? persistentData, VersionConfig? streamingAssets, CancellationToken cancellationToken = default)
        {
            if (server == null)
            {
                "无法加载到 Server 下的版本文件".FrameError();
                return UniTask.FromResult(false);
            }
            if (streamingAssets == null)
            {
                "无法找到架构版本, 请下载最新的安装包".FrameError();
                return UniTask.FromResult(false);
            }
            if (server.FrameVersion != streamingAssets.FrameVersion)
            {
                "架构版本不是最新, 请下载最新的安装包".FrameError();
                return UniTask.FromResult(false);
            }
            if (persistentData != null)
            {
                if (server.HotUpdateVersion == persistentData.HotUpdateVersion)
                {
                    $"已更新到最新版本, 无需更新!!".FrameLog();
                    return UniTask.FromResult(false);
                }
            }
            return UniTask.FromResult(true);
        }
    }
}