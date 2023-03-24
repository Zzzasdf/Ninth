using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace Ninth.Editor
{
    public partial class BrowserOperation : UnityEditor.Editor
    {
        [MenuItem("Tools/Browser/Joy/网易云音乐")]
        private static void WebsiteWangYiYunMusic()
        {
            Process.Start("https://music.163.com/#");
        }

        [MenuItem("Tools/Browser/Joy/bilibili")]
        private static void WebsiteBilibili()
        {
            Process.Start("https://www.bilibili.com/");
        }

        [MenuItem("Tools/Browser/Joy/Age动漫")]
        private static void Website()
        {
            Process.Start("https://www.agemys.net/");
        }
    }
}