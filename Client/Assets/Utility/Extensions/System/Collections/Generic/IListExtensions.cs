using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Collections.Generic
{
    public static class IListExtensions
    {
        public static bool IsSafetyRange<T>(this IList<T> list, int index)
        {
            if (index >= 0 && index < list.Count) return true;
            $"越界!! 可选范围: [0 .. {list.Count - 1}], 当前错误索引值：{index}".FrameError();
            return false;
        }
    }
}
