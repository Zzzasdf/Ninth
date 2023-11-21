using Ninth.Editor.Excel.Search;
using System;
using System.Collections.Generic;

namespace Ninth.Editor.Excel
{
    public class ExcelSettingsData: IExcelSettingsData
    {
        public Dictionary<ExcelMode, Action> TabDic { get; } = new Dictionary<ExcelMode, Action>()
        {
            [ExcelMode.Encode] = new ExcelEncodeSettings().OnGUI,
            [ExcelMode.Search] = new SearchSettings().OnGUI,
        };

        public ExcelMode ExcelMode
        {
            get => WindowSOCore.Get<WindowCollectConfig>().ExcelMode;
            set => WindowSOCore.Get<WindowCollectConfig>().ExcelMode = value;
        }
    }
}