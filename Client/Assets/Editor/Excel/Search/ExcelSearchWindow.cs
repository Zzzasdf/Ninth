using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{

    public sealed partial class ExcelSearchWindow : EditorWindow
    {
        private void Awake()
        {
            if(m_FoldOutDic != null)
            {
                List<string> keys = m_FoldOutDic.Keys.ToList();
                for(int index = 0; index < keys.Count; index++)
                {
                    m_FoldOutDic[keys[index]] = true;
                }
            }
            if(m_FoldOutDic2 != null)
            {
                List<string> keys = m_FoldOutDic2.Keys.ToList();
                for (int index = 0; index < keys.Count; index++)
                {
                    if (m_FoldOutDic2[keys[index]] != null)
                    {
                        List<string> keys2 = m_FoldOutDic2[keys[index]].Keys.ToList();
                        for (int i = 0; i < keys2.Count; i++)
                        {
                            m_FoldOutDic2[keys[index]][keys2[i]] = true;
                        }
                    }
                }
            }
            m_FilterFunc = (SearchMode)PlayerPrefsDefine.ExcelSearchMode switch
            {
                SearchMode.Exact => ExactFunc,
                SearchMode.Exist => ExistFunc,
                _ => throw new System.NotImplementedException(),
            };
        }

        private void OnGUI()
        {
            // SetSearchDirectory
            SetSearchDirectory();

            // Compile
            SetCompile();

            if(m_Compile != null)
            {
                // SearchObj
                SetSearchObj();

                // SearchSettings
                ExchangeSearchMode();
                SetSearch();

                // SearchResult
                ExchangeSearchResultMode();
                GUILayout.Label(m_SearchResultTypeInfo, EditorStyles.boldLabel);
                if(m_SearchResult != null)
                {
                    SetSearchResult();
                }
                GUILayout.Label(m_SearchResultIntro, EditorStyles.boldLabel);
            }
        }
    }
}
