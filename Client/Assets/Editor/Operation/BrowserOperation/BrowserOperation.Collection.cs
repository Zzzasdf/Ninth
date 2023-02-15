using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace NinthEditor
{
    public partial class BrowserOperation : Editor
    {
        [MenuItem("Tools/Browser/Collection/SharpLab")]
        private static void WebsiteSharpLab()
        {
            Process.Start("https://sharplab.io/");
        }

        [MenuItem("Tools/Browser/Collection/Unity手册")]
        private static void WebsiteUnityDocs()
        {
            Process.Start("https://docs.unity3d.com/cn");
        }
    }
}