using System;
using System.Linq;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class ViewLayout: MonoBehaviour
    {
        private RectTransform bg;
        private RectTransform main;
        private RectTransform frame;
        private RectTransform tip;
        private RectTransform pop;

        public void Awake()
        {
            var rects = GetComponentsInChildren<RectTransform>().ToDictionary(value => value.name, value => value);
            bg = rects["Bg"];
            main = rects["Main"];
            frame = rects["Frame"];
            tip = rects["Tip"];
            pop = rects["Pop"];
        }

        public RectTransform? GetViewHierarchy(VIEW_HIERARCHY viewHierarchy)
        {
            return viewHierarchy switch
            {
                VIEW_HIERARCHY.Bg => bg,
                VIEW_HIERARCHY.Main => main,
                VIEW_HIERARCHY.Frame => frame,
                VIEW_HIERARCHY.Tip => tip,
                VIEW_HIERARCHY.Pop => pop,
                _ => null
            };
        }
    }
}
