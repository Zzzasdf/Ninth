using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ninth
{
    public class GlobalConfig
    {
        /// <summary>
        /// 编码格式
        /// </summary>
        public static UTF8Encoding Utf8 { get; private set; }

        /// <summary>
        /// 资源加载模式
        /// </summary>
        public static AssetMode AssetMode { get; private set; }

        /// <summary>
        /// 模块列表Url
        /// </summary>
        public static string Url { get; private set; }

        static GlobalConfig()
        {
            Utf8 = new UTF8Encoding(false);

            AssetMode = AssetMode.RemoteAB;

            Url = "http://192.168.1.192:80";
        }
    }
}