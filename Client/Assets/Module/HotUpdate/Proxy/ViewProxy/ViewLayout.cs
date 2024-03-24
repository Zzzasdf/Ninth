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
            var rects = GetComponentsInChildren<RectTransform>().ToList();
            bg = rects.Find(x => x.name == "Bg");
            main = rects.Find(x => x.name == "Main");
            frame = rects.Find(x => x.name == "Frame");
            tip = rects.Find(x => x.name == "Tip");
            pop = rects.Find(x => x.name == "Pop");
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
