using UnityEngine;
using UnityEditor;

public class FindPrefabsWithButton : EditorWindow
{
    [MenuItem("Tools/Find Prefabs with Button Component")]
    static void Init()
    {
        FindPrefabsWithButton window = (FindPrefabsWithButton)EditorWindow.GetWindow(typeof(FindPrefabsWithButton));
        window.Show();
    }

    void OnGUI()
    {
        if(GUILayout.Button("Find Prefabs with Button Component"))
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach(string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if(obj != null && obj.GetComponentInChildren<UnityEngine.UI.Button>() != null)
                {
                    Debug.Log(path);
                }
            }
        }
    }
}
