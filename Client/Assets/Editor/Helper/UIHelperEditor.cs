using Ninth.HotUpdate;
using UnityEditor;

namespace Ninth.Editor
{
    [CustomEditor(typeof(UIHelper), true)]
    public class UIHelperEditor : IHelperEditor<UIHelper, UIHelperBarMode, UIHelperMap>
    {
        protected override UIHelperBarMode TEnumBitPlus(UIHelperBarMode tEnum1, UIHelperBarMode tEnum2)
        {
            return tEnum1 | tEnum2;
        }
    }
}