using Ninth.Utility;
using UnityEditor;

namespace Ninth.Editor
{
    public class BuildDllInfo: BaseBuildInfo
    {
        public BuildDllInfo(AssetGroup assetGroup)
        {
            this.assetGroup = assetGroup;
        }
        
        public override void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget)
        {
            
        }

        public override void CalculateDependencies()
        {
        }
    }
}
