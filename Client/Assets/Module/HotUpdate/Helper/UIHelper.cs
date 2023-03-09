using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public enum UIHelperBarMode
    {
        Button = 1 << 0,
        Text = 1 << 1,
    }

    public class UIHelperMap : IHelperMap<UIHelperBarMode, UIHelperMap>
    {
        protected override void Assembler()
        {
            Assembler(
                (UIHelperBarMode.Button, typeof(Button)),
                (UIHelperBarMode.Text, typeof(Text))
                );
        }
    }

    public class UIHelper : IHelper<UIHelperBarMode, UIHelperMap>
    {
        public override UIHelperMap Map()
        {
            return UIHelperMap.Instance;
        }
    }
}