using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public class ApplyOperation : Editor
{
    [MenuItem("Tools/App/Work/��Ѷ����")]
    private static void App()
    {
        Process.Start("F:/��Ѷ����/WeMeet/wemeetapp.exe");
    }
}
