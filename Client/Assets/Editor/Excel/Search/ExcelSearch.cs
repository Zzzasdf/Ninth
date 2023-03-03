using UnityEditor;

namespace Ninth.Editor
{
    public class ExcelSearch
    {
        [MenuItem("Tools/Excels/Search")]
        private static void OpenExcel()
        {
            EditorWindow.GetWindow<ExcelSearchWindow>();
        }
    }
}