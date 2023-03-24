using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace Ninth.Editor
{
    public partial class BrowserOperation : UnityEditor.Editor
    {
        [MenuItem("Tools/Browser/百度")]
        private static void WebsiteBaiDu()
        {
            Process.Start("http://www.baidu.com");
        }
    }
}
