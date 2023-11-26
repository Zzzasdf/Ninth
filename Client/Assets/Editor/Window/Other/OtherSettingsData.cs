using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class OtherSettingsData: IOtherSettingsData
    {
        public Dictionary<string, Dictionary<string, string>> AppOperationDic { get; } = new Dictionary<string, Dictionary<string, string>>()
        {
            ["腾讯会议"] = new Dictionary<string, string>()
            {
                ["Open"] = "F:/腾讯会议/WeMeet/wemeetapp.exe",
            },
            ["网易云音乐"] = new Dictionary<string, string>()
            {
                ["Open"] = "D:/网易云音乐/CloudMusic/cloudmusic.exe",
            },
        };

        public Dictionary<string, Dictionary<string, string>> BrowserOperationDic { get; } = new Dictionary<string, Dictionary<string, string>>()
        {
            ["百度"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://www.baidu.com",
            },
            ["SharpLab"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://sharplab.io/",
            },
            ["Unity手册"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://docs.unity3d.com/cn/current/Manual/UnityManual.html",
            },
            ["日报"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://doc.weixin.qq.com/sheet/e3_AUkA7gZjAMMGprHaH0vSG6W6a6Vlb?scode=AJEAIQdfAAoPheliJhAZQA9AYHAEU&tab=vp2xxk",
            },
            ["周报"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://docs.qq.com/sheet/DZWxzQkliUmt2Ylpu?from_account_guide=1&tab=l2hsbx",
            },
            ["版本开发计划"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://doc.weixin.qq.com/sheet/e3_AUkA7gZjAMMWw0eD6r1SKO2Lt9W33?scode=AJEAIQdfAAo5fbXa0pAZQA9AYHAEU&tab=hbwx0f",
            },
            ["GM文档"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://docs.qq.com/sheet/DZVZ4b1F0eUFrcktJ?tab=BB08J2",
            },
            ["TAPD需求"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://tapd.tencent.com/tapd_fe/10124851/story/list?categoryId=-1&sort_name=priority&order=desc&useScene=storyList&conf_id=1110124851041679764&page=1&queryToken=53ae62eef74241f1635fc6c0cbf223bf",
            },
            ["TAPD缺陷"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://tapd.tencent.com/tapd_fe/10124851/bug/list?confId=1110124851041741684",
            },
            ["bilibili"] = new Dictionary<string, string>()
            {
                ["Open"] = "https://www.bilibili.com/",
            },
        };

        public Dictionary<string, Dictionary<string, Action>> DirectoryOperationDic { get; } = new Dictionary<string, Dictionary<string, Action>>()
        {
            ["StreamingAssets"] = new Dictionary<string, Action>()
            {
                ["Open"] = () => Application.OpenURL(Application.streamingAssetsPath),
                ["Clear"] = () =>
                {
                    Directory.Delete(Application.streamingAssetsPath, true);
                    AssetDatabase.Refresh();
                },
            },
            ["PersistentData"] = new Dictionary<string, Action>()
            {
                ["Open"] = () => Application.OpenURL(Application.persistentDataPath),
                ["Clear"] = () =>
                {
                    Directory.Delete(Application.persistentDataPath, true);
                    AssetDatabase.Refresh();
                },
            },
            ["OutputBundles"] = new Dictionary<string, Action>()
            {
                ["Open"] = () => Application.OpenURL(WindowSOCore.Get<WindowBuildConfig>().BuildBundlesTargetFolderRoot),
            },
            ["OutputPlayer"] = new Dictionary<string, Action>()
            {
                ["Open"] = () => Application.OpenURL(WindowSOCore.Get<WindowBuildConfig>().BuildPlayersDirectoryRoot),
            },
            ["AllCache"] = new Dictionary<string, Action>()
            {
                ["Clear"] = () =>
                {
                    Directory.Delete(Application.persistentDataPath, true);
                    PlayerPrefs.DeleteAll();
                    AssetDatabase.Refresh();
                },
            }
        };

        public bool AllFoldout
        {
            set
            {
                AppFoldout = value;
                BrowserFoldout = value;
                DirectoryFoldout = value;
            }
        }

        public bool AppFoldout
        {
            get => WindowSOCore.Get<WindowOtherConfig>().AppFoldout;
            set => WindowSOCore.Get<WindowOtherConfig>().AppFoldout = value;
        }

        public bool BrowserFoldout
        {
            get => WindowSOCore.Get<WindowOtherConfig>().BrowserFoldout;
            set => WindowSOCore.Get<WindowOtherConfig>().BrowserFoldout = value;
        }

        public bool DirectoryFoldout
        {
            get => WindowSOCore.Get<WindowOtherConfig>().DirectoryFoldout;
            set => WindowSOCore.Get<WindowOtherConfig>().DirectoryFoldout = value;
        }
    }
}
