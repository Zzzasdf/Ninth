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

    public enum BuildBundleMode
    {
        HotUpdateBundles,
        AllBundles
    }

    public class BuildJson : IJson
    {
        public string? ExportBundleFolder { get; set; }
        public string? ExportPlayFolder { get; set; }
        
        public List<string> LocalGroup { get; set; } = new();
        public List<string> RemoteGroup { get; set; } = new();

        public BuildTargetPlatform BuildTargetPlatforms { get; set; }
        public SerializableDictionary<BuildTargetPlatform, VersionJson> PlatformVersions { get; set; } = new();
    }
}