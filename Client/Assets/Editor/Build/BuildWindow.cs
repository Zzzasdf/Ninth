using System;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class BuildWindowParam
    {
        public bool IsBase { get; set; }
        public bool IsRemote { get; set; }
        public Func<string, bool> DelegateConfirm { get; set; }

        public void Show()
        {
            BuildWindow.Param = this;
            EditorWindow.GetWindow<BuildWindow>();
        }
    }

    public class BuildWindow : EditorWindow
    {
        public static BuildWindowParam Param;

        private int m_BigVersion;
        private int m_SmallVersion;
        private int m_HotUpdateVersion;

        private int m_BaseIteration;
        private int m_HotUpdateIteration;

        private void Awake()
        {
            m_BigVersion = PlayerPrefsDefine.BigBaseVersion;
            m_SmallVersion = PlayerPrefsDefine.SmallBaseVersion;
            m_HotUpdateVersion = PlayerPrefsDefine.HotUpdateVersion;

            m_HotUpdateIteration = PlayerPrefsDefine.HotUpdateIteration;
            m_BaseIteration = PlayerPrefsDefine.BaseIteration;
        }

        private void OnGUI()
        {
            GUILayout.Label("Please confirm the generated version number ?", EditorStyles.boldLabel);
            SetSourceDataDirectoryRoot();
            SetVersion();
            SetButtonConfirm();
        }

        private void SetSourceDataDirectoryRoot()
        {
            GUILayout.Space(20);
            GUILayout.Label("Output Path Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.BuildBundlesDirectoryRoot = EditorGUILayout.TextField("BundleSourceDataDirectoryRoot", PlayerPrefsDefine.BuildBundlesDirectoryRoot);
            GUI.enabled = true;
            if(GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select a folder to store resources", PlayerPrefsDefine.BuildBundlesDirectoryRoot, "Bundles");
                if(!string.IsNullOrEmpty(path))
                {
                    PlayerPrefsDefine.BuildBundlesDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.BuildPlayersDirectoryRoot = EditorGUILayout.TextField("PlayerSourceDataDirectoryRoot", PlayerPrefsDefine.BuildPlayersDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select a folder to store resources", PlayerPrefsDefine.BuildPlayersDirectoryRoot, "Players");
                if(!string.IsNullOrEmpty(path))
                {
                    PlayerPrefsDefine.BuildPlayersDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetVersion()
        {
            GUILayout.Space(20);
            GUILayout.Label("Version Settings", EditorStyles.boldLabel);
            SetBaseVersion();
            SetHotUpdateVersion();
            SetIteration();
        }

        private void SetBaseVersion()
        {
            if (Param.IsBase)
            {
                m_BigVersion = EditorGUILayout.IntField("BigVersion", m_BigVersion);
                if(m_BigVersion < PlayerPrefsDefine.BigBaseVersion)
                {
                    m_BigVersion = PlayerPrefsDefine.BigBaseVersion;
                }
                
                if (m_BigVersion == PlayerPrefsDefine.BigBaseVersion)
                {
                    if(m_SmallVersion <= PlayerPrefsDefine.SmallBaseVersion)
                    {
                        m_SmallVersion = PlayerPrefsDefine.SmallBaseVersion + 1;
                    }
                    m_SmallVersion = EditorGUILayout.IntField("SmallVersion", m_SmallVersion);
                }
                else
                {
                    GUI.enabled = false;
                    m_SmallVersion = EditorGUILayout.IntField("SmallVersion", 0);
                    GUI.enabled = true;
                }
            }
            else
            {
                GUI.enabled = false;
                m_BigVersion = EditorGUILayout.IntField("BigVersion", PlayerPrefsDefine.BigBaseVersion);
                m_SmallVersion = EditorGUILayout.IntField("SmallVersion", PlayerPrefsDefine.SmallBaseVersion);
                GUI.enabled = true;
            }
        }

        private void SetHotUpdateVersion()
        {
            if (Param.IsBase)
            {
                GUI.enabled = false;
                m_HotUpdateVersion = EditorGUILayout.IntField("HotUpdateVersion", 0);
                GUI.enabled = true;
            }
            else
            {
                if (m_HotUpdateVersion <= PlayerPrefsDefine.HotUpdateVersion)
                {
                    m_HotUpdateVersion = PlayerPrefsDefine.HotUpdateVersion + 1;
                }
                m_HotUpdateVersion = EditorGUILayout.IntField("HotUpdateVersion", m_HotUpdateVersion);
            }
        }

        private void SetIteration()
        {
            GUI.enabled = false;
            if (Param.IsBase)
            {
                m_BaseIteration = EditorGUILayout.IntField("BaseIteration", PlayerPrefsDefine.BaseIteration + 1);
                m_HotUpdateIteration = EditorGUILayout.IntField("HotUpdateIteration", PlayerPrefsDefine.HotUpdateIteration);
            }
            else
            {
                m_BaseIteration = EditorGUILayout.IntField("BaseIteration", PlayerPrefsDefine.BaseIteration);
                m_HotUpdateIteration = EditorGUILayout.IntField("HotUpdateIteration", PlayerPrefsDefine.HotUpdateIteration + 1);
            }
            GUI.enabled = true;
        }

        private void SetButtonConfirm()
        {
            if (GUILayout.Button("Confirm"))
            {
                ButtonClick();
                GetWindow<BuildWindow>().Close();
            }
            if(Param.IsRemote && GUILayout.Button("Confirm And Apply"))
            {
                ButtonClick(()=> BuildAssetsCommand.RemoteApply());
                GetWindow<BuildWindow>().Close();
            }

            void ButtonClick(Action action = null)
            {
                string newVersion = string.Join(".", m_BigVersion, m_SmallVersion, m_HotUpdateVersion, m_BaseIteration, m_HotUpdateIteration);
                bool? result = Param.DelegateConfirm?.Invoke(newVersion);
                if (result == null || result == false)
                {
                    UnityEngine.Debug.LogError("Packaging Failed !!");
                }
                else
                {
                    PlayerPrefsDefine.BigBaseVersion = m_BigVersion;
                    PlayerPrefsDefine.SmallBaseVersion = m_SmallVersion;
                    PlayerPrefsDefine.HotUpdateVersion = m_HotUpdateVersion;
                    PlayerPrefsDefine.BaseIteration = m_BaseIteration;
                    PlayerPrefsDefine.HotUpdateIteration = m_HotUpdateIteration;
                    if (Param.IsRemote)
                    {
                        action?.Invoke();
                    }
                }
            }
        }
    }
}

