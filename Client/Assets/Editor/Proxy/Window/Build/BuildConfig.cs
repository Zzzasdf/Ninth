using Ninth.Utility;
using UnityEditor;
using VContainer;

namespace Ninth.Editor
{
    public class BuildConfig : IBuildConfig
    {
        private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
        private readonly CommonSubscribe<BuildDirectoryRoot, string> stringCommonSubscribe;
        private readonly CommonSubscribe<BuildVersion, int> intCommonSubscribe;

        EnumTypeSubscribe<int> IBuildConfig.IntEnumTypeSubscribe => intEnumTypeSubscribe;
        CommonSubscribe<BuildDirectoryRoot, string> IBuildConfig.StringCommonSubscribe => stringCommonSubscribe;
        CommonSubscribe<BuildVersion, int> IBuildConfig.IntCommonSubscribe => intCommonSubscribe;
        
        [Inject]
        public BuildConfig(BuildJson buildJson)
        {
            {
                var build = intEnumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<BuildSettingsMode>((int)buildJson.BuildSettingsType).AsSetEvent(value => buildJson.BuildSettingsType = (BuildSettingsMode)value);
                build.Subscribe<BuildBundleMode>((int)buildJson.BuildBundleMode).AsSetEvent(value => buildJson.BuildBundleMode = (BuildBundleMode)value);
                build.Subscribe<BuildExportCopyFolderMode>((int)buildJson.BuildExportDirectoryType).AsSetEvent(value => buildJson.BuildExportDirectoryType = (BuildExportCopyFolderMode)value);
                build.Subscribe<ActiveTargetMode>((int)buildJson.ActiveTargetMode).AsSetEvent(value => buildJson.ActiveTargetMode = (ActiveTargetMode)value);
                build.Subscribe<BuildTarget>((int)buildJson.BuildTarget).AsSetEvent(value => buildJson.BuildTarget = (BuildTarget)value);
                build.Subscribe<BuildTargetGroup>((int)buildJson.BuildTargetGroup).AsSetEvent(value => buildJson.BuildTargetGroup = (BuildTargetGroup)value);
            }

            {
                var build = stringCommonSubscribe = new CommonSubscribe<BuildDirectoryRoot, string>();
                build.Subscribe(BuildDirectoryRoot.Players, buildJson.BuildPlayersDirectoryRoot).AsSetEvent(value => buildJson.BuildPlayersDirectoryRoot = value);
                build.Subscribe(BuildDirectoryRoot.Bundles, buildJson.BuildBundlesDirectoryRoot).AsSetEvent(value => buildJson.BuildBundlesDirectoryRoot = value);
                build.Subscribe(BuildDirectoryRoot.DisaplayVerison, buildJson.DisplayVersion).AsSetEvent(value => buildJson.DisplayVersion = value);
            }

            {
                var build = intCommonSubscribe = new CommonSubscribe<BuildVersion, int>();
                build.Subscribe(BuildVersion.Frame, buildJson.FrameVersion).AsSetEvent(value => buildJson.FrameVersion = value);
                build.Subscribe(BuildVersion.HotUpdate, buildJson.HotUpdateVersion).AsSetEvent(value => buildJson.HotUpdateVersion = value);
                build.Subscribe(BuildVersion.Iterate, buildJson.IterateVersion).AsSetEvent(value => buildJson.IterateVersion = value);
            }
        }
    }
}