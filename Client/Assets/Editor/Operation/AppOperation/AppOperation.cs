using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public class ApplyOperation : Editor
{
    [MenuItem("Tools/App/Work/腾讯会议")]
    private static void App()
    {
        Process.Start("F:/腾讯会议/WeMeet/wemeetapp.exe");
    }
}
