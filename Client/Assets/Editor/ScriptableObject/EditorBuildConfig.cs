using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "BuildConfigSO", menuName = "EditorConfig/BuildConfigSO")]
    public class EditorBuildConfig : ScriptableObject
    {
        [SerializeField] private string buildBundlesDirectoryRoot;
        [SerializeField] private string buildPlayersDirectoryRoot;
        [SerializeField] private BuildSettingsType buildSettingsType;
        [SerializeField] private BuildBundleMode buildBundleMode;
        [SerializeField] private BuildPlayerMode buildPlayerMode;
        [SerializeField] private int majorVersion;
        [SerializeField] private int minorVersion;
        [SerializeField] private int revisionNumber;
        [SerializeField] private BuildExportDirectoryType buildExportDirectoryType;
        [SerializeField] private ActiveTargetMode activeTargetMode;
        [SerializeField] private BuildTarget buildTarget;
        [SerializeField] private BuildTargetGroup buildTargetGroup;

        public string BuildBundlesDirectoryRoot
        {
            get => buildBundlesDirectoryRoot;
            set => SetProperty(ref buildBundlesDirectoryRoot, value);
        }
        public string BuildPlayersDirectoryRoot
        {
            get => buildPlayersDirectoryRoot;
            set => SetProperty(ref buildPlayersDirectoryRoot, value);
        }
        public BuildSettingsType BuildSettingsType
        {
            get => buildSettingsType;
            set => SetProperty(ref buildSettingsType, value);
        }
        public BuildBundleMode BuildBundleMode
        {
            get => buildBundleMode;
            set => SetProperty(ref buildBundleMode, value);
        }
        public BuildPlayerMode BuildPlayerMode
        {
            get => buildPlayerMode;
            set => SetProperty(ref buildPlayerMode, value);
        }
        public int MajorVersion
        {
            get => majorVersion;
            set => SetProperty(ref majorVersion, value);
        }
        public int MinorVersion
        {
            get => minorVersion;
            set => SetProperty(ref minorVersion, value);
        }
        public int RevisionNumber
        {
            get => revisionNumber;
            set => SetProperty(ref revisionNumber, value);
        }
        public BuildExportDirectoryType BuildExportDirectoryType
        {
            get => buildExportDirectoryType;
            set => SetProperty(ref buildExportDirectoryType, value);
        }
        public ActiveTargetMode ActiveTargetMode
        {
            get => activeTargetMode;
            set => SetProperty(ref activeTargetMode, value);
        }
        public BuildTarget BuildTarget
        {
            get => buildTarget;
            set => SetProperty(ref buildTarget, value);
        }
        public BuildTargetGroup BuildTargetGroup
        {
            get => buildTargetGroup;
            set => SetProperty(ref buildTargetGroup, value);
        }

        private void OnEnable()
        {
            SetDefaultBuildBundlesDirectoryRoot();
            SetDefaultBuildPlayersDirectoryRoot();
        }

        private void SetDefaultBuildBundlesDirectoryRoot()
        {
            if(string.IsNullOrEmpty(BuildBundlesDirectoryRoot))
            {
                BuildBundlesDirectoryRoot = $"{Application.dataPath}/../../Bundles";
            }
        }

        private void SetDefaultBuildPlayersDirectoryRoot()
        {
            if(string.IsNullOrEmpty(BuildPlayersDirectoryRoot))
            {
                buildPlayersDirectoryRoot = $"{Application.dataPath}/../../Players";
            }
        }

        private void SetProperty<T>(ref T field, T value)
        {
            if (!Equals(field, value))
            {
                field = value;
                SaveAssets();
            }
        }

        private void SaveAssets()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

    public enum BuildSettingsType
    {
        Bundle,
        Player
    }

    public enum BuildBundleMode
    {
        HotUpdateBundles,
        AllBundles
    }

    public enum BuildPlayerMode
    {
        InoperationBundle,
        RepackageBundle
    }

    public enum BuildExportDirectoryType
    {
        Local,
        Remote
    }
    
    public enum ActiveTargetMode
    {
        ActiveTarget,
        InactiveTarget
    }
}