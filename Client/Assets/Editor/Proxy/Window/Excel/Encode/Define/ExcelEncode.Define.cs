//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//namespace Ninth.Editor.Window
//{
//    public partial class ExcelEncodeSettings
//    {
//        // 分隔符 ===============================================
//        private readonly static string Separator = "#";

//        // 1、表头定义 ===========================================
//        // 行字段定义（解析）
//        // 字段名，是否在每张表里必须显示, 格式
//        private static Dictionary<string, List<string>> RowFieldDic = new Dictionary<string, List<string>>()
//        {
//            ["Type"] = classFormat, // 不可删除
//            ["Name"] = fieldNameFormat, // 不可删除
//            ["Desc"] = summaryFormat,
//        };

//        // 列字段定义（解析）
//        private static string Key = "Key";
//        // 列字段定义（限制）
//        private static Dictionary<string, Func<string, List<string>, bool>> ColFieldDic = new Dictionary<string, Func<string, List<string>, bool>>()
//        {
//            ["Unique"] = IsUnqine,
//        };

//        // 2、数据类型定义 =======================================
//        private static Dictionary<string, string> TypeDic = new Dictionary<string, string>()
//        {
//            ["List"] = "List<{0}>",
//            ["Array"] = "{0}[]",
//        };

//    }

//    // 类型结构解析
//    public class TypeStructureAnalysis
//    {
//        private string type;
//        private List<string> typeWrapper;

//        private (bool success, string errorLog) TryParse(string cellType)
//        {
//            if(!string.IsNullOrEmpty(cellType))
//            {
                
//            }
//            return (false, string.Format("无法解析此单元格类型: {0}", cellType));
//        }
//    }
//}
