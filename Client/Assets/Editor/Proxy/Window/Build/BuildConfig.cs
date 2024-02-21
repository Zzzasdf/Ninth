using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Ninth.Editor.Window
{
    [Serializable]
    public class BuildConfig: IBuildConfig
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
            enumTypeSubscribe = new EnumTypeSubscribe<int>()
                .Subscribe<BuildSettingsMode>((int)buildSettingsType)
                .Subscribe<BuildBundleMode>((int)buildBundleMode)
                .Subscribe<BuildExportCopyFolderMode>((int)buildExportDirectoryType)
                .Subscribe<ActiveTargetMode>((int)activeTargetMode)
                .Subscribe<BuildTarget>((int)buildTarget)
                .Subscribe<BuildTargetGroup>((int)buildTargetGroup);

            commonStringSubscribe = new CommonSubscribe<BuildDirectoryRoot, string>
            {
                [BuildDirectoryRoot.Players] = buildPlayersDirectoryRoot,
                [BuildDirectoryRoot.Bundles] = buildBundlesDirectoryRoot,
            };

            commonIntSubscribe = new CommonSubscribe<BuildVersion, int>
            {
                [BuildVersion.Frame] = frameVersion,
                [BuildVersion.HotUpdate] = hotUpdateVersion,
                [BuildVersion.Iterate] = iterateVersion,
            };
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