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
