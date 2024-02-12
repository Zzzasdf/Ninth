using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ninth
{
    public class MessageBox
    {
        private DownloadProxy DownloadProxy { get; }

        public MessageBox(DownloadProxy downloadProxy)
        {
            this.DownloadProxy = downloadProxy;
            Init();
        }

        private void Init()
        {
            node = GameObject.Instantiate(Resources.Load<GameObject>("LoadView/Prefabs/MessageBox"));
            Dictionary<string, Button> btnDic = node.transform.GetComponentsInChildren<Button>()
                .ToDictionary(value => value.name, value => value);
            Dictionary<string, Text> txtDic = node.transform.GetComponentsInChildren<Text>()
                .ToDictionary(value => value.name, value => value);
            txtMessage = txtDic["txtMessage"];
            btnFirst = btnDic["btnFirst"];
            txtFirst = txtDic["txtFirst"];
            btnFirst.onClick.AddListener(() =>
            {
                btnFirstClicked = true;
                btnFirst.gameObject.SetActive(false);
                btnSecond.gameObject.SetActive(false);
            });
            btnSecond = btnDic["btnSecond"];
            txtSecond = txtDic["txtSecond"];
            btnSecond.onClick.AddListener(() =>
            {
                btnSecondClicked = true;
                btnFirst.gameObject.SetActive(false);
                btnSecond.gameObject.SetActive(false);
            });
        }

        private GameObject node;
        private Text txtMessage;
        private Button btnFirst;
        private Text txtFirst;
        private bool btnFirstClicked;

        private Button btnSecond;
        private Text txtSecond;
        private bool btnSecondClicked;

        private string stringFormat;
        private long curPackPercent;
        private long curPackSize;


        public async Task<ProcedureInfo> DownloadBeforce(string strMessage, string strFirst,
            Func<ProcedureInfo> btnFirstClickedFunc, string strSecond, Func<ProcedureInfo> btnSecondClickedFunc)
        {
            txtMessage.text = strMessage;
            txtFirst.text = strFirst;
            txtSecond.text = strSecond;
            btnFirst.gameObject.SetActive(true);
            btnSecond.gameObject.SetActive(true);

            while (!btnFirstClicked && !btnSecondClicked)
            {
                Debug.Log("等待用户点击中...");
                await Task.Delay(100); // 每次等待0.1秒
            }

            if (btnFirstClicked)
            {
                return btnFirstClickedFunc.Invoke();
            }

            return btnSecondClickedFunc.Invoke();
        }

        public void DownloadNext(string stringFormat, long size)
        {
            this.stringFormat = stringFormat;
            curPackSize = size;
        }

        public void CancelRefreshProgress()
        {
        }

        public void OverStatus()
        {
            txtMessage.text = string.Format(stringFormat, 100.ToString(),
                DownloadProxy.GetCompleteDownloadIncreaseBundleAmount, DownloadProxy.GetAllIncreaseBundleCount);
            Clear();
        }

        private void RefreshProgress(float deltaTime)
        {
            long curSize = DownloadProxy.GetCompleteDownloadIncreaseBundleProgress +
                           (long)(curPackSize * DownloadProxy.Progress);
            long totalSize = DownloadProxy.GetTotalIncreaseBundleSize;
            if (totalSize == 0)
            {
                return;
            }

            // 百分比显示
            curPackPercent = curSize * 100 / totalSize;
            if (stringFormat != null)
            {
                txtMessage.text = string.Format(stringFormat, curPackPercent.ToString(),
                    DownloadProxy.GetCompleteDownloadIncreaseBundleAmount, DownloadProxy.GetAllIncreaseBundleCount);
            }
        }

        private void Clear()
        {
            UnityEngine.Object.Destroy(node);
        }
    }
}