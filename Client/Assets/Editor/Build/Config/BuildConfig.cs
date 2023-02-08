using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildConfig : Editor
{
    public string GAssetNode =>
        string.Format("{0}/GAssets", Application.dataPath);

    public static List<string> GetLocalGroups =>
        new List<string> { "LocalGroup" };

    public static List<string> GetRemoteGroups =>
        new List<string> { "RemoteGroup" };

}
