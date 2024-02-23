using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public enum BuildDirectoryRoot
    {
        Players,
        Bundles,
        DisaplayVerison,
    }

    public enum BuildVersion
    {
        Frame,
        HotUpdate,
        Iterate
    }

    public enum BuildSettingsMode
    {
        Bundle,
        Player
    }

    public enum BuildBundleMode
    {
        HotUpdateBundles,
        AllBundles
    }

    public enum BuildExportCopyFolderMode
    {
        StreamingAssets,
        Remote
    }

    public enum ActiveTargetMode
    {
        ActiveTarget,
        InactiveTarget
    }
    
    public class BuildJson : IJson
    {
        public BuildSettingsMode BuildSettingsType { get; set; }
        public BuildBundleMode BuildBundleMode { get; set; }
        public BuildExportCopyFolderMode BuildExportDirectoryType { get; set; }
        public ActiveTargetMode ActiveTargetMode { get; set; }
        public BuildTarget BuildTarget { get; set; }
        public BuildTargetGroup BuildTargetGroup { get; set; }

        public string BuildPlayersDirectoryRoot { get; set; }
        public string BuildBundlesDirectoryRoot { get; set; }
        public string DisplayVersion { get; set; }

        public int FrameVersion { get; set; }
        public int HotUpdateVersion { get; set; }
        public int IterateVersion { get; set; }
    }
}