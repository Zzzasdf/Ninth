using System;
using System.Collections.Generic;
using System.IO;

namespace Ninth.Editor.Excel.Search
{
    using Cell = TableData.Table.Sheet.Cell;

    public class TableData
    {
        public Dictionary<string, Table> Tables { get; private set; }
        public Dictionary<string, bool> TableFolders { get; set; }

        public TableData()
        {
            Tables = new Dictionary<string, Table>();
            TableFolders = new Dictionary<string, bool>();
        }

        public TableData AddTable(FileInfo fileInfo)
        {
            try
            {
                Tables.Add(fileInfo.FullName, new Table(this, fileInfo.FullName, fileInfo.Name));
                return this;
            }
            catch
            {
                throw new ArgumentException(nameof(Tables));
            }
        }

        public TableData AddCell(Cell cell)
        {
            if (!Tables.TryGetValue(cell.Sheet.Table.FullName, out Table value))
            {
                value = new Table(this, cell.Sheet.Table.FullName, cell.Sheet.Table.Name);
                Tables.Add(cell.Sheet.Table.FullName, value);
            }
            value.AddCell(cell);
            return this;
        }

        public bool GetFolder(string tableFullName)
        {
            TableFolders.TryGetValue(tableFullName, out bool value);
            return value;
        }

        public void SetFolder(string tableFullName, bool value)
        {
            if (!TableFolders.ContainsKey(tableFullName))
            {
                TableFolders.Add(tableFullName, value);
            }
            else
            {
                TableFolders[tableFullName] = value;
            }
        }

        // 表
        public class Table
        {
            public TableData CompileResult { get; private set; }
            public string FullName { get; private set; }
            public string Name { get; private set; }
            public Dictionary<int, Sheet> Sheets { get; private set; }
            public Dictionary<int, bool> SheetFolder { get; private set; }
            
            public Table(TableData compileResult, string fullName, string name)
            {
                CompileResult = compileResult ?? throw new ArgumentNullException(nameof(compileResult));
                FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
                Sheets = new Dictionary<int, Sheet>();
                SheetFolder = new Dictionary<int, bool>();
                Name = name;
            }
            
            public Table AddSheet(int sheetIndex)
            {
                try
                {
                    Sheets.Add(sheetIndex, new Sheet(this, sheetIndex));
                    return this;
                }
                catch
                {
                    throw new ArgumentException(nameof(Sheets));
                }
            }
            
            public Table AddCell(Cell cell)
            {
                if (!cell.Sheet.Table.FullName.Equals(FullName)
                    || !cell.Sheet.Table.Name.Equals(Name))
                {
                    UnityEngine.Debug.LogErrorFormat("此Cell{0}属于该表{1}, 不属于表{2}", nameof(cell), cell.Sheet.Table.FullName, FullName);
                    return this;
                }
                if (!Sheets.TryGetValue(cell.Sheet.SheetIndex, out Sheet value))
                {
                    value = new Sheet(this, cell.Sheet.SheetIndex);
                    Sheets.Add(cell.Sheet.SheetIndex, value);
                }
                value.AddCell(cell.Row, cell.Col, cell.Value);
                return this;
            }

            // 页
            public class Sheet
            {
                public Table Table { get; private set; }
                public int SheetIndex { get; private set; }
                public Dictionary<(int, int), Cell> Cells { get; private set; }
                public Dictionary<int, List<Cell>> RowCells { get; private set; }
                public Dictionary<int, List<Cell>> ColCells { get; private set; }
                
                public Sheet(Table table, int sheetIndex)
                {
                    Table = table ?? throw new ArgumentNullException(nameof(table));
                    SheetIndex = sheetIndex;
                    Cells = new Dictionary<(int, int), Cell>();
                    RowCells = new Dictionary<int, List<Cell>>();
                    ColCells = new Dictionary<int, List<Cell>>();
                }

                
                public Sheet AddCell(int row, int col, string value)
                {
                    try
                    {
                        Cell cell = new Cell(this, row, col, value);
                        Cells.Add((row, col), cell);
                        if (!RowCells.ContainsKey(row))
                        {
                            RowCells.Add(row, new List<Cell>());
                        }
                        RowCells[row].Add(cell);
                        if (!ColCells.ContainsKey(col))
                        {
                            ColCells.Add(col, new List<Cell>());
                        }
                        ColCells[col].Add(cell);
                        return this;
                    }
                    catch
                    {
                        throw new ArgumentException(nameof(Cells));
                    }
                }

                // 格子
                public class Cell
                {
                    public Sheet Sheet { get; private set; }
                    public int Row { get; private set; }
                    public int Col { get; private set; }
                    public string Value { get; private set; }

                    public Cell(Sheet sheet, int row, int col, string value)
                    {
                        Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
                        Row = row;
                        Col = col;
                        Value = value;
                    }
                }
            }
        }
    }
    }
