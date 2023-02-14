using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class PackApplyWindow : EditorWindow
    {
        private void Awake()
        {

        }

        private void OnGUI()
        {
            GUILayout.Label("Whether to apply this version ?");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                GetWindow<PackApplyWindow>().Close();
            }
            if(GUILayout.Button("Apply"))
            {
                BuildAssetsCommand.RemoteApply();
                GetWindow<PackApplyWindow>().Close();
                UnityEngine.Debug.Log("Apply Success！！");
            }
            GUILayout.EndHorizontal();
        }
    }
}
