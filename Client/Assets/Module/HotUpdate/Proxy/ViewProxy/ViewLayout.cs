using System;
using System.Linq;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class ViewLayout: MonoBehaviour
    {
        [SerializeField] private RectTransform bg;
        [SerializeField] private RectTransform main;
        [SerializeField] private RectTransform frame;
        [SerializeField] private RectTransform tip;
        [SerializeField] private RectTransform pop;

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
