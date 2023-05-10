using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class ExcelEncode
    {
        public void OnDraw()
        {
            SetEncodeDirectory();
            SetCompile();
        }
    }
}