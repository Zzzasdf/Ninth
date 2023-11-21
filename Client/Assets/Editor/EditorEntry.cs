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
        public static ConfigCore Config { get; } // 配置
        public static DownloadCore Download { get; private set; } // 下载
        public static JsonCore Json { get; private set; } // Json
        public static ProcedureCore Procedure { get; private set; } // 流程

        public static PackConfig PackConfig { get; }
        public static BuildAssetsCommand BuildAssetsCmd { get; }

        static EditorEntry()
        {
            Config = new ConfigCore();
            Download = new DownloadCore();
            Json = new JsonCore(Config.Encoding);
            Procedure = new ProcedureCore(Config.AssetConfig, Config.PathConfig, Download);

            PackConfig = new PackConfig(Config.PlatformConfig, Config.NameConfig, Config.PathConfig);
            BuildAssetsCmd = new BuildAssetsCommand(Config.AssetConfig, PackConfig, Json);
        }
    }
}