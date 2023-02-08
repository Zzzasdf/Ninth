using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public partial class BrowserOperation : Editor
{
    [MenuItem("Tools/Browser/°Ù¶È")]
    private static void WebsiteBaiDu()
    {
        Process.Start("http://www.baidu.com");
    }
}
