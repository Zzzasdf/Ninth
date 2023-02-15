using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace NinthEditor
{
    public partial class BrowserOperation : Editor
    {
        [MenuItem("Tools/Browser/百度")]
        private static void WebsiteBaiDu()
        {
            Process.Start("http://www.baidu.com");
        }
    }
}
