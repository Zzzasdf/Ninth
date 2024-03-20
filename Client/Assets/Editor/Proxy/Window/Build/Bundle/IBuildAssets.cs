using Ninth.Utility;
using UnityEditor;

namespace Ninth.Editor
{
    public interface IBuildAssets
    {
        AssetGroupsPaths? AssetGroupsPaths { get; }
        void ScanAssets(string buildFolder, BuildTarget buildTarget);
        void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget);
        void CalculateDependencies();
        void SaveConfig(string fullFolder, IJsonProxy jsonProxy, string loadConfigName, string? downloadConfigName);
    }
}

