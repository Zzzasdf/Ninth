using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Ninth
{
    public partial class GameEntry : MonoBehaviour
    {
        public static GameEntry Instance { get; private set; }

        public static DownloadCore DownloadCore;
        private void Awake()
        {
            Instance = this;
            DownloadCore = new DownloadCore();
        }

        void Start()
        {
            new Launcher().EnterProcedure();
        }

        [SerializeField, Header("正则表达式")] private string regex;
        [SerializeField, Header("测试字符串")] private string regStr;
        void Test()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                // Regex reg = new Regex(@regex);
                // Match match = reg.Match(regStr);
                // UnityEngine.Debug.LogFormat("Value匹配输出结果[{0}]", match.Value.Trim());
                // UnityEngine.Debug.LogFormat("ToString匹配输出结果[{0}]", match.ToString().Trim());
                if(byte.TryParse(regStr, out byte result))
                {
                    UnityEngine.Debug.LogFormat("[{0}]", result.ToString());
                }
                else
                {
                    UnityEngine.Debug.Log("转换失败!");
                }
            }
        }
    }
}