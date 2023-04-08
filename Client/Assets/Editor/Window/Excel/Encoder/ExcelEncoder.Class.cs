using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelEncoder
    {
        private static string AddRowSummary(string summary)
        {
            StringBuilder sb = new StringBuilder();
            return sb.AppendLine("        /// <summary>")
                .AppendLine($"        /// {summary}")
                .AppendLine("        /// </summary>")
                .ToString();
        }

        private static string AddRowField(string typeName, string fieldName)
        {
            return string.Format("        public {0} {1} { get; private set; }", typeName, fieldName);
        }

        //private static string AddRowConstructor(params (string typeName, string fieldName)[] values)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int index = 0; index < values.Length; index++)
        //    {
        //        sb.AppendLine($"            {values[index].fieldName} = new {values[index].typeName}();");
        //    }
        //}
    }
}