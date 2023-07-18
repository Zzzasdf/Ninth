using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public interface IExcelSettingsData
    {
        Dictionary<ExcelMode, Action> TabDic { get; }
        ExcelMode ExcelMode { get; set; }
    }
}