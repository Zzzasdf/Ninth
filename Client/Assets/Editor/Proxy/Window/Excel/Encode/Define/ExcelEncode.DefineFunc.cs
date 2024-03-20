using System.Collections.Generic;

namespace Ninth.Editor.Window
{
    public partial class ExcelEncodeSettings
    {
        // 1、表头定义 ===========================================
        // 行字段定义（解析）
        private static List<string> classFormat = new List<string>()
        {
            "{0}",
        };

        private static List<string> fieldNameFormat = new List<string>()
        {
            "{0}",
        };
        
        private static List<string> summaryFormat = new List<string>()
        {
            "/// <summary>",
            "/// {0}",
            "/// </summary>",
        };

        // 2、数据类型定义 ==========================================
        private static bool IsUnqine(string cell, List<string> cells)
        {
            if(cells == null)
            {
                return true;
            }
            return !cell.Contains(cell);
        }
    }
}