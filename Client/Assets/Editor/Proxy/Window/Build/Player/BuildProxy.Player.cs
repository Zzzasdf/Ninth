using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildProxy
    {
        private void BuildPlayer(BuildPlayersConfig buildPlayersConfig)
        {
            var folder = buildPlayersConfig.BuildFolder;
            var prefixFolder = buildPlayersConfig.PlayerPrefix;
            var produceName = buildPlayersConfig.ProduceName;
            var buildOptions = buildPlayersConfig.BuildTargetPlatformInfo.BuildOptions;
            var buildTarget = buildPlayersConfig.BuildTarget;
            var buildTargetGroup = buildPlayersConfig.BuildTargetGroup;
            
            var location = $"{folder}/{prefixFolder}/{produceName}.exe";
            Debug.Log("====> Build App");
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new [] { "Assets/Resources/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = buildTarget,
                targetGroup = buildTargetGroup,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("客户端生成失败，请检查！！");
            }
        }
    }
}
