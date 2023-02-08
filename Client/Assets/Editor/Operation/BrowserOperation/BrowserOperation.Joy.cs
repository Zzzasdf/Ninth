using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public partial class BrowserOperation : Editor
{
    [MenuItem("Tools/Browser/Joy/����������")]
    private static void WebsiteWangYiYunMusic()
    {
        Process.Start("https://music.163.com/#");
    }

    [MenuItem("Tools/Browser/Joy/bilibili")]
    private static void WebsiteBilibili()
    {
        Process.Start("https://www.bilibili.com/");
    }

    [MenuItem("Tools/Browser/Joy/Age����")]
    private static void Website()
    {
        Process.Start("https://www.agemys.net/");
    }
}
