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
        public static PackConfig PackConfig { get; }
        public static BuildAssetsCommand BuildAssetsCmd { get; }
    }
}