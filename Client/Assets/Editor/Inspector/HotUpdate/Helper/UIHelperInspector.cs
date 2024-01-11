using Ninth.HotUpdate;
using UnityEditor;

namespace Ninth.Editor
{
    [CustomEditor(typeof(UIHelper), true)]
    public class UIHelperInspector : BaseHelperInspector<UIHelper, UIHelperBarMode, UIHelperMap>
    {
        protected override UIHelperBarMode TEnumBitAnd(UIHelperBarMode tEnum1, UIHelperBarMode tEnum2)
        {
            return tEnum1 | tEnum2;
        }
    }
}