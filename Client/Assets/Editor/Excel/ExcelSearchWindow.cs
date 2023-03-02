using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{

    public sealed partial class ExcelSearchWindow : EditorWindow
    {
        private void Awake()
        {
            // Compile
            //SetCompileValue();

            // Input
            m_SearchObjList = new List<string>();
            m_Count = 1; // 默认一个

            m_SearchObjResultList = new List<string>();
            m_SearchResult = new List<SearchResultCell>();
            m_SearchResultDic = new Dictionary<string, Dictionary<string, LinkedList<SearchResultCell>>>();
            m_FoldOutDic = new Dictionary<string, bool>();
            m_FoldOutDic2 = new Dictionary<string, Dictionary<string, bool>>();

            m_FilterFunc = (SearchMode)PlayerPrefsDefine.ExcelSearchMode switch
            {
                SearchMode.Exact => ExactFunc,
                SearchMode.Exist => ExistFunc,
                _ => throw new System.NotImplementedException(),
            };
            m_SearchResultTypeInfo = string.Empty;
            m_SearchResultTypeInfo = string.Empty;
        }

        private void OnGUI()
        {
            // ExcelPath
            SetExcelPathDirectoryRoot();

            // Compile
            //SetExcelCompilePathDirectoryRoot();
            SetCompile();

            if(m_Compile != null)
            {
                // Input
                SetSearchObj();

                // SearchSettings
                ExchangeSearchMode();
                SetSearch();

                // SearchResult
                ExchangeSearchResultMode();
                GUILayout.Label(m_SearchResultTypeInfo);
                SetSearchResult();
                GUILayout.Label(m_SearchResultIntro);
            }
        }
    }
}
