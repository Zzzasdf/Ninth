using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.Editor.Window
{
    // 正则定义
    public partial class ExcelEncodeSettings
    {
        private Dictionary<string, Func<string, string>> CommonExpCollect = new Dictionary<string, Func<string, string>>()
        {
            ["byte"] = (txt) => byte.TryParse(txt, out byte result) ? result.ToString() : string.Empty,
            ["int"] = (txt) => int.TryParse(txt, out int result) ? result.ToString() : string.Empty,
            ["float"] = (txt) => float.TryParse(txt, out float result) ? result.ToString() : string.Empty,
            ["double"] = (txt) => double.TryParse(txt, out double result) ? result.ToString() : string.Empty,
        };
    }
}
