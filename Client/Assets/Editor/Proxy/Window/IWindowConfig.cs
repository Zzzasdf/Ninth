using System;
using UnityEngine;

namespace Ninth.Editor.Window
{
    public enum Tab
    {
        Build,
        Excel,
        Scan,
        Review,
        Other
    }
    
    public interface IWindowConfig
    {
        Tab Tab { get; set; }
    }
}