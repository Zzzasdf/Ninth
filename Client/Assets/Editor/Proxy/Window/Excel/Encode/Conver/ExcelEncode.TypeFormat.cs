using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor.Window
{
    public partial class ExcelEncodeSettings
    {
        private Dictionary<string, string> typeFormat = new Dictionary<string, string>()
        {
            ["int"] = "{0}",
            ["float"] = "{0}",
            ["double"] = "{0}",
            ["string"] = "{0}",
            ["FuncType"] = "FuncType.{0}",
        };

        private (bool bValid, string format) GetTypeFormat(string type)
        {
            if(typeFormat.TryGetValue(type, out string value))
            {
                return (true, value);
            }
            return (false, null);
        }
    }
}
