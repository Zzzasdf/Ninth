using System;
using System.Collections.Generic;

namespace Ninth.Editor.Window
{
    public interface IOtherConfig
    {
        Dictionary<string, Dictionary<string, string>> AppOperationDic { get; }
        Dictionary<string, Dictionary<string, string>> BrowserOperationDic { get; }
        Dictionary<string, Dictionary<string, Action>> DirectoryOperationDic { get; }
        bool AllFoldout { set; }
        bool AppFoldout { get; set; }
        bool BrowserFoldout { get; set; }
        bool DirectoryFoldout { get; set; }
    }
}

