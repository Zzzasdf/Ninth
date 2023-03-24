using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "BuildConfigSO", menuName = "EditorConfig/BuildConfigSO")]
    public class EditorBuildConfig : ScriptableObject
    {
        public string BuildBundlesDirectoryRoot;
        public string BuildPlayersDirectoryRoot;

        public BuildSettingsType BuildSettingsType;
        public BuildBundleMode BuildBundleMode;
        public BuildPlayerMode BuildPlayerMode;

        public int BigBaseVersion;
        public int SmallBaseVersion;
        public int HotUpdateVersion;
        public int BaseIteration;
        public int HotUpdateIteration;

        public BuildExportDirectoryType BuildExportDirectoryType;
        public ActiveTargetMode ActiveTargetMode;
        public BuildTarget BuildTarget;
        public BuildTargetGroup BuildTargetGroup;

        private void OnEnable()
        {
            if(BuildBundlesDirectoryRoot == null)
            {
                BuildBundlesDirectoryRoot = $"{Application.dataPath}/../../Bundles";
            }
            if(BuildPlayersDirectoryRoot == null)
            {
                BuildPlayersDirectoryRoot = $"{Application.dataPath}/../../Players";
            }
        }

        private void OnValidate()
        {
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