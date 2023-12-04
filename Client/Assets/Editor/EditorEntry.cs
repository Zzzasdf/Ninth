using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConfigCore = Ninth.GameEntry.ConfigCore;
using DownloadCore = Ninth.GameEntry.DownloadCore;
using JsonCore = Ninth.GameEntry.JsonCore;
using ProcedureCore = Ninth.GameEntry.ProcedureCore;

namespace Ninth.Editor
{
    public class EditorEntry
    {
        public static GameEntry GameEntry => Object.FindObjectOfType<GameEntry>();
        public static ConfigCore Config => GameEntry.Config; // 配置
        public static DownloadCore Download => GameEntry.Download; // 下载
        public static JsonCore Json => GameEntry.Json;  // Json
        public static ProcedureCore Procedure => GameEntry.Procedure; // 流程

        public static PlatformConfig PlatformConfig => new PlatformConfig();
        public static NameConfig NameConfig => new NameConfig();
        public static PathConfig PathConfig => new PathConfig(Config.AssetConfig, PlatformConfig, NameConfig);
        public static PackConfig PackConfig => new PackConfig(PlatformConfig, NameConfig, PathConfig);
        public static BuildAssetsCommand BuildAssetsCmd => new BuildAssetsCommand(Config.AssetConfig, PackConfig, Json);
    }
}