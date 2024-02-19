using System;
using UnityEngine;

namespace Ninth.Editor.Window
{
    [Serializable]
    public class WindowConfig: IWindowConfig
    {
        [SerializeField] private Tab tab;

        public Tab Tab
        {
            get => tab;
            set => tab = value;
        }
    }
}