using UnityEngine;

namespace Ninth.HotUpdate
{
    public class ViewLayout: MonoBehaviour
    {
        [SerializeField] private RectTransform? bg;
        [SerializeField] private RectTransform? main;
        [SerializeField] private RectTransform? frame;
        [SerializeField] private RectTransform? tip;
        [SerializeField] private RectTransform? pop;

        public RectTransform? GetViewHierarchy(ViewHierarchy viewHierarchy)
        {
            return viewHierarchy switch
            {
                ViewHierarchy.Bg => bg,
                ViewHierarchy.Main => main,
                ViewHierarchy.Frame => frame,
                ViewHierarchy.Tip => tip,
                ViewHierarchy.Pop => pop,
                _ => null
            };
        }
    }
}
