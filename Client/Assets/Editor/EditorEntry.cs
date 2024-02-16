using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public class EditorEntry
    {
        public static ConfigCore Config; // 配置
        public static DownloadProxy Download; // 下载
        public static JsonProxy Json;  // Json
        public static ProcedureProxy Procedure; // 流程

        public static PlayerSettingsConfig PlayerSettingsConfig => new PlayerSettingsConfig();
        public static NameConfig NameConfig => new NameConfig();
        public static PathConfig PathConfig => new PathConfig(Config.AssetConfig, PlayerSettingsConfig, NameConfig);
        public static PackConfig PackConfig => new PackConfig(PlayerSettingsConfig, NameConfig, PathConfig);
        public static BuildAssetsCommand BuildAssetsCmd => new BuildAssetsCommand(Config.AssetConfig, PackConfig, Json);
    }
}