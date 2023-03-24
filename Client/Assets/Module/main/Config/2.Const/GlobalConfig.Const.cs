using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ninth
{
    public partial class GlobalConfig
    {
        /// <summary>
        /// 编码格式
        /// </summary>
        public UTF8Encoding Encoding { get; private set; } = new UTF8Encoding(false);
    }
}