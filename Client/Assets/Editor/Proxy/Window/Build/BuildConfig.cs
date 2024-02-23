using System;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Ninth.Editor
{
    [Serializable]
    public class BuildConfig : IBuildConfig
    {
        [SerializeField] private BuildSettingsMode buildSettingsType;
        [SerializeField] private BuildBundleMode buildBundleMode;
        [SerializeField] private BuildExportCopyFolderMode buildExportDirectoryType;
        [SerializeField] private ActiveTargetMode activeTargetMode;
        [SerializeField] private BuildTarget buildTarget;
        [SerializeField] private BuildTargetGroup buildTargetGroup;

        [SerializeField] private string buildPlayersDirectoryRoot;
        [SerializeField] private string buildBundlesDirectoryRoot;
        [SerializeField] private string displayVersion;

        [SerializeField] private int frameVersion;
        [SerializeField] private int hotUpdateVersion;
        [SerializeField] private int iterateVersion;

        private readonly EnumTypeSubscribe<int> enumTypeSubscribe;
        private readonly CommonSubscribe<BuildDirectoryRoot, string> commonStringSubscribe;
        private readonly CommonSubscribe<BuildVersion, int> commonIntSubscribe;

        [Inject]
        public BuildConfig()
        {
            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<BuildSettingsMode>((int)buildSettingsType);
                build.Subscribe<BuildBundleMode>((int)buildBundleMode);
                build.Subscribe<BuildExportCopyFolderMode>((int)buildExportDirectoryType);
                build.Subscribe<ActiveTargetMode>((int)activeTargetMode);
                build.Subscribe<BuildTarget>((int)buildTarget);
                build.Subscribe<BuildTargetGroup>((int)buildTargetGroup);
            }

            {
                var build = commonStringSubscribe = new CommonSubscribe<BuildDirectoryRoot, string>();
                build.Subscribe(BuildDirectoryRoot.Players, buildPlayersDirectoryRoot);
                build.Subscribe(BuildDirectoryRoot.Bundles, buildBundlesDirectoryRoot);
            }

            {
                var build = commonIntSubscribe = new CommonSubscribe<BuildVersion, int>();
                build.Subscribe(BuildVersion.Frame, frameVersion);
                build.Subscribe(BuildVersion.HotUpdate, hotUpdateVersion);
                build.Subscribe(BuildVersion.Iterate, iterateVersion);
            }
        }
    }

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
}