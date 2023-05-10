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
    public sealed partial class ExcelSearch
    {
        private ExcelSearchMode ExcelSearchMode
        {
            get => EditorSOCore.GetExcelConfig().ExcelSearchMode;
            set => EditorSOCore.GetExcelConfig().ExcelSearchMode = value;
        }
        private bool m_Init;

        public void OnDraw()
        {
            if (!m_Init)
            {
                m_Init = !m_Init;
                Init();
            }

            // SetSearchDirectory
            SetSearchDirectory();

            // Compile
            SetCompile();

            if(m_Compile != null)
            {
                // SearchInputObj
                SetSearchInputObj();

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

        private void Init()
        {
            if (m_FoldOutDic != null)
            {
                List<string> keys = m_FoldOutDic.Keys.ToList();
                for (int index = 0; index < keys.Count; index++)
                {
                    m_FoldOutDic[keys[index]] = true;
                }
            }
            if (m_FoldOutDic2 != null)
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
            m_FilterFunc = ExcelSearchMode switch
            {
                ExcelSearchMode.Exact => ExactFunc,
                ExcelSearchMode.Exist => ExistFunc,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
