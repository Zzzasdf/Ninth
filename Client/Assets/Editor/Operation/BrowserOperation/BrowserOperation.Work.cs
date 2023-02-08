using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public partial class BrowserOperation: Editor
{
    [MenuItem("Tools/Browser/Work/�ձ�")]
    private static void BrowsersiteDailyPaper()
    {
        Process.Start("https://docs.qq.com/sheet/DRFZIdFVRVHhCZWFo?from_account_guide=1&tab=juetvi&_t=1670580029173");
    }

    [MenuItem("Tools/Browser/Work/�ܱ�")]
    private static void BrowsersiteWeekPaper()
    {
        Process.Start("https://docs.qq.com/sheet/DZWxzQkliUmt2Ylpu?from_account_guide=1&tab=l2hsbx");
    }

    [MenuItem("Tools/Browser/Work/GM�ĵ�")]
    private static void BrowsersiteGMDocs()
    {
        Process.Start("https://docs.qq.com/sheet/DZVZ4b1F0eUFrcktJ?tab=BB08J2");
    }

    [MenuItem("Tools/Browser/Work/TAPD .. ȱ��")]
    private static void BrowsersiteTAPDBug()
    {
        Process.Start("https://tapd.tencent.com/tapd_fe/10124851/bug/list?confId=1110124851041741684");
    }

    [MenuItem("Tools/Browser/Work/�汾�����ƻ�")]
    private static void BrowsersiteVersionQPN()
    {
        Process.Start("https://doc.weixin.qq.com/sheet/e3_AUkA7gZjAMMWw0eD6r1SKO2Lt9W33?scode=AJEAIQdfAAovvfK1BeAUkA7gZjAMM&version=4.0.20.6020&platform=win&tab=hbwx0f");
    }
}
