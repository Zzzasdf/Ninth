using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public enum AssetGroup
    {
        Local,
        Remote,
    }
    public enum BuildFolder
    {
        Players,
        Bundles,
    }

    public enum BuildVersion
    {
        Display,
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
        public Common BuildCommon { get; private set; } = new();
        public Bundle BuildBundle { get; private set; } = new();
        public Player BuildPlayer { get; private set; } = new();

        public class Common
        {
            public List<string> LocalGroup { get; set; } = new();
            public List<string> RemoteGroup { get; set; } = new();
            public int CurrentBuildModeIndex { get; set; }
            public string DisplayVersion { get; set; }
            public int HotUpdateVersion { get; set; }
            public int IterateVersion { get; set; }
        }
        
        public class Bundle
        {
            public string ExportBundleFolder { get; set; }
            public int CurrentExportBundleModeIndex { get; set; }
            public int CurrentCopyBundleModeIndex { get; set; }
            public string CopyBundleRemotePath { get; set; }
            public int CurrentActiveTargetModeIndex { get; set; }
            public int InactiveBuildTargetIndex { get; set; }
        }

        public class Player
        {
            public string ExportBundleFolder { get; set; }
            public string ExportPlayFolder { get; set; }
            public int CurrentExportBundleModeIndex { get; set; }
            public int CurrentCopyBundleModeIndex { get; set; }
            public string CopyBundleRemotePath { get; set; }
            public int CurrentActiveTargetModeIndex { get; set; }
            public int InactiveBuildTargetIndex { get; set; }
            public int FrameVersion { get; set; }
        }
    }
}