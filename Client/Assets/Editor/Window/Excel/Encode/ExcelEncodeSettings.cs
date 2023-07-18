using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class ExcelEncodeSettings
    {
        public void OnGUI()
        {
            SetEncodeDirectory();
            // SetDisplay();
        }
    }
}