using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public enum BuildFolder
    {
        Players,
        Bundles,
    }

    public enum BuildSettingsMode
    {
        HotUpdateBundle,
        AllBundle,
        Player
    }

    public enum BuildBundleOperate
    {
        ClearStreamingAssets,
        Copy2StreamingAssets,
    }
    
    public class BuildJson : IJson
    {
        public string? ExportBundleFolder { get; set; }
        public string? ExportPlayFolder { get; set; }
        
        public List<string> LocalGroup { get; set; } = new();
        public List<string> RemoteGroup { get; set; } = new();

        public BuildTargetPlatform BuildTargetPlatforms { get; set; }
        public BuildSettingsMode BuildSettingsMode { get; set; }
        public SerializableDictionary<BuildTargetPlatform, BuildTargetPlatformInfo> PlatformVersions { get; set; } = new();
    }

    public class BuildTargetPlatformInfo: VersionJson
    {
        // 压缩选项详解
        // BuildAssetBundleOptions.None：使用LZMA算法压缩，压缩的包更小，但是加载时间更长。使用之前需要整体解压。一旦被解压，这个包会使用LZ4重新压缩。使用资源的时候不需要整体解压。在下载的时候可以使用LZMA算法，一旦它被下载了之后，它会使用LZ4算法保存到本地上。
        // BuildAssetBundleOptions.UncompressedAssetBundle：不压缩，包大，加载快
        // BuildAssetBundleOptions.ChunkBasedCompression：使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部
        public BuildAssetBundleOptions BuildAssetBundleOptions { get; set; }
        public BuildOptions BuildOptions { get; set; }
        public int BuildBundleOperateIndex { get; set; }
        public bool BundleCopy2Player { get; set; }
    }
}