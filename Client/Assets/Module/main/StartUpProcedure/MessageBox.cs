using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ninth
{
    public class MessageBox
    {
        private GameObject m_Node;

        private Button m_BtnFirst;

        private Button m_BtnSecond;

        private Text m_TxtMessage;

        private Text m_TxtFirst;

        private Text m_TxtSecond;

        private string m_StringFormat;

        private long m_CurPackPercent;

        private long m_CurPackSize;

        public MessageBox()
        {
            m_Node = GameObject.Instantiate(Resources.Load<GameObject>("LoadView/Prefabs/MessageBox"));

            Dictionary<string, Button> btnDic = m_Node.transform.GetComponentsInChildren<Button>().ToDictionary(value => value.name, value => value);

            m_BtnFirst = btnDic["btnFirst"];

            m_BtnSecond = btnDic["btnSecond"];

            Dictionary<string, Text> txtDic = m_Node.transform.GetComponentsInChildren<Text>().ToDictionary(value => value.name, value => value);

            m_TxtMessage = txtDic["txtMessage"];

            m_TxtFirst = txtDic["txtFirst"];

            m_TxtSecond = txtDic["txtSecond"];
        }

        public void DownloadBeforce(string txtMessage, (UnityAction btnFirst, string txtFirst) First, (UnityAction btnSecond, string txtSecond) Second)
        {
            m_TxtMessage.text = txtMessage;

            m_BtnFirst.onClick.RemoveAllListeners();

            First.btnFirst += () =>
            {
                m_BtnFirst.gameObject.SetActive(false);

                m_BtnSecond.gameObject.SetActive(false);
            };
            m_BtnFirst.onClick.AddListener(First.btnFirst);

            m_TxtFirst.text = First.txtFirst;

            m_BtnSecond.onClick.RemoveAllListeners();

            m_BtnSecond.onClick.AddListener(Second.btnSecond);

            m_TxtSecond.text = Second.txtSecond;

            m_BtnFirst.gameObject.SetActive(true);

            m_BtnSecond.gameObject.SetActive(true);
        }

        public void DownloadNext(string stringFormat, long size)
        {
            m_StringFormat = stringFormat;

            m_CurPackSize = size;

            GameEntry.RegisterUpdate(RefreshProgress);
        }

        public void CancelRefreshProgress()
        {
            GameEntry.CancelUpdate(RefreshProgress);
        }

        public void OverStatus()
        {
            m_TxtMessage.text = string.Format(m_StringFormat, 100.ToString(), GameEntry.DownloadCore.GetCompleteDownloadIncreaseBundleAmount, GameEntry.DownloadCore.GetAllIncreaseBundleCount);

            Clear();
        }

        private void RefreshProgress(float deltaTime)
        {
            long curSize = GameEntry.DownloadCore.GetCompleteDownloadIncreaseBundleProgress + (long)(m_CurPackSize * GameEntry.DownloadCore.Progress);

            long totalSize = GameEntry.DownloadCore.GetTotalIncreaseBundleSize;

            if(totalSize == 0)
            {
                return;
            }
            // 百分比显示
            m_CurPackPercent = curSize * 100 / totalSize;

            if(m_StringFormat != null)
            {
                m_TxtMessage.text = string.Format(m_StringFormat, m_CurPackPercent.ToString(), GameEntry.DownloadCore.GetCompleteDownloadIncreaseBundleAmount, GameEntry.DownloadCore.GetAllIncreaseBundleCount);
            }
        }

        private void Clear()
        {
            UnityEngine.Object.Destroy(m_Node);
        }
    }
}

