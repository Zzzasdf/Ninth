using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public interface IOtherSettingsData
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

