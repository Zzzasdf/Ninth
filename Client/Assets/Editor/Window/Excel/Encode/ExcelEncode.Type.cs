using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelEncode
    {
        public enum CollectType
        { 
            None,
            Array,
            List
        }

        public class ExcelColInfo
        {
            public string FieldName { get; set; }
            public bool IsNullable { get; set; }
            public CollectType CollectType { get; set; }
            public int Count { get; set; }
            public string Type { get; set; }

            public ExcelColInfo(string excelType, string excelField)
            {
                FieldName = excelField;

                // IsNullable
                if (excelField.Contains("?"))
                {
                    IsNullable = true;
                }

                // CollectType
                if (excelField.Contains("[")
                    && excelField.Contains("]"))
                {
                    CollectType = CollectType.Array;
                    int leftSymbol = excelField.IndexOf("[");
                    int rightSymbol = excelField.IndexOf("]");
                    string count = excelField[(leftSymbol + 1) .. rightSymbol];
                    Count = int.Parse(count);
                    Type = excelField[ .. leftSymbol];
                }
                else if (excelField.Contains("<")
                    && excelField.Contains(">"))
                {
                    CollectType = CollectType.List;
                    Count = 0;
                }
                else
                {
                    CollectType = CollectType.None;
                }
            }
        }
    }
}
