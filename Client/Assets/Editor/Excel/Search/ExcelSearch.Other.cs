using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        public class CompileTable
        {
            public string TableName;
            public string Path;
            public List<CompileSheet> CompileSheets;
            public CompileTable()
            {
                CompileSheets = new List<CompileSheet>();
            }
        }

        public class CompileSheet
        {
            public int SheetIndex;
            public List<CompileCell> Cells;
            public CompileSheet()
            {
                Cells = new List<CompileCell>();
            }
        }

        public class CompileCell
        {
            public int Row;
            public int Col;
            public string CellValue;
        }

        public class SearchResultCell: CompileCell
        {
            public string SearchValue;
            public string Path;
            public int SheetIndex;
        }

        public class SearchCompile
        {
            public List<CompileTable> Tables;

            public SearchCompile()
            {
                Tables = new List<CompileTable>();
            }
        }
    }
}