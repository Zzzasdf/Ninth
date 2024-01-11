using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    public enum UIHelperBarMode
    {
        Canvas = 1 << 0,
        Text = 1 << 1,
        Image = 1 << 2,
        Button = 1 << 3,
        Toggle = 1 << 4,
        Slider = 1 << 5,
        ScrollView = 1 << 6,
        Dropdown = 1 << 7,
        InputField = 1 << 8,

        Object = 1 << 31,
    }

    public class UIHelperMap : BaseHelperMap<UIHelperBarMode, UIHelperMap>
    {
        protected override void Assembler()
        {
            Assembler(
                (UIHelperBarMode.Canvas, typeof(Canvas)),
                (UIHelperBarMode.Text, typeof(Text)),
                (UIHelperBarMode.Image, typeof(Image)),
                (UIHelperBarMode.Button, typeof(Button)),
                (UIHelperBarMode.Toggle, typeof(Toggle)),
                (UIHelperBarMode.Slider, typeof(Slider)),
                (UIHelperBarMode.ScrollView, typeof(UnityEngine.UIElements.ScrollView)),
                (UIHelperBarMode.Dropdown, typeof(Dropdown)),
                (UIHelperBarMode.InputField, typeof(InputField)),
                (UIHelperBarMode.Object, typeof(UnityEngine.Object))
                );
        }
    }

    public class UIHelper : BaseHelper<UIHelperBarMode, UIHelperMap>
    {
        public override UIHelperMap Map()
        {
            return UIHelperMap.Instance;
        }
    }
}