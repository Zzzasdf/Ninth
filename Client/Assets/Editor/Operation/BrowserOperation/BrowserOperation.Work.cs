using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace NinthEditor
{
    public partial class BrowserOperation : Editor
    {
        [MenuItem("Tools/Browser/Work/日报")]
        private static void BrowsersiteDailyPaper()
        {
            Process.Start("https://docs.qq.com/sheet/DRFZIdFVRVHhCZWFo?from_account_guide=1&tab=pj4pdf&_t=1670580029173");
        }

        [MenuItem("Tools/Browser/Work/日报1")]
        private static void BrowsersiteDailyPaper1()
        {
            Process.Start("https://doc.weixin.qq.com/sheet/e3_AUkA7gZjAMMGprHaH0vSG6W6a6Vlb?scode=AJEAIQdfAAoPheliJhAZQA9AYHAEU&tab=l5iz9j");
        }

        [MenuItem("Tools/Browser/Work/周报")]
        private static void BrowsersiteWeekPaper()
        {
            Process.Start("https://docs.qq.com/sheet/DZWxzQkliUmt2Ylpu?from_account_guide=1&tab=l2hsbx");
        }

        [MenuItem("Tools/Browser/Work/GM文档")]
        private static void BrowsersiteGMDocs()
        {
            Process.Start("https://docs.qq.com/sheet/DZVZ4b1F0eUFrcktJ?tab=BB08J2");
        }

        [MenuItem("Tools/Browser/Work/TAPD .. 缺陷")]
        private static void BrowsersiteTAPDBug()
        {
            Process.Start("https://tapd.tencent.com/tapd_fe/10124851/bug/list?confId=1110124851041741684");
        }

        [MenuItem("Tools/Browser/Work/版本开发计划")]
        private static void BrowsersiteVersionQPN()
        {
            Process.Start("https://doc.weixin.qq.com/sheet/e3_AUkA7gZjAMMWw0eD6r1SKO2Lt9W33?scode=AJEAIQdfAAo5fbXa0pAZQA9AYHAEU&tab=hbwx0f");
        }
    }
}