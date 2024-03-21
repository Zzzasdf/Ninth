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

        public RectTransform? GetViewHierarchy(VIEW_HIERARCY viewHierarcy)
        {
            return viewHierarcy switch
            {
                VIEW_HIERARCY.Bg => bg,
                VIEW_HIERARCY.Main => main,
                VIEW_HIERARCY.Frame => frame,
                VIEW_HIERARCY.Tip => tip,
                VIEW_HIERARCY.Pop => pop,
                _ => null
            };
        }
    }
}
