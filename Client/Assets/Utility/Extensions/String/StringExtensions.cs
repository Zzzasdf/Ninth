using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public static class StringExtensions
    {
        public static string[] ToArrayString<T>(this ICollection<T> tArray)
        {
             var result = new string[tArray.Count];
             var i = 0;
             foreach (var item in tArray)
             {
                 result[i] = item?.ToString() ?? string.Empty;
                 i++;
             }
             return result;
        }

        public static int[] ToIndexArray<T>(this ICollection<T> tArray)
        {
            var result = new int[tArray.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = i;
            }
            return result;
        }
        
        
        
        #region Format
        public static string Format(this string s, string arg0, string arg1)
        {
            return string.Format("{0}/{1}", arg0, arg1);
        }
        public static string Format(this string s, string arg0, string arg1, string arg2)
        {
            return string.Format("{0}/{1}/{2}", arg0, arg1, arg2);
        }
        public static string Format(this string s, string arg0, string arg1, string arg2, string arg3)
        {
            return string.Format("{0}/{1}/{2}/{3}", arg0, arg1, arg2, arg3);
        }
        public static string Format(this string s, string arg0, string arg1, string arg2, string arg3, string arg4)
        {
            return string.Format("{0}/{1}/{2}/{3}/{4}", arg0, arg1, arg2, arg3, arg4);
        }
        #endregion

        public static int ToInt(this string s, int defaultValue = 0)
        {
            return int.TryParse(s, out int i) ? i : defaultValue;
        }

        public static string AddColor(this string s, string color)
        {
            return $"<color={color}>{s}</color>";
        }

        //public static StringBuilder Append(this string s, string value)
        //{
        //    return StringBuilderPool.Get().Append(s).Append(value);
        //}

        //public static StringBuilder AppendLine(this string s, string value)
        //{
        //    return StringBuilderPool.Get().AppendLine(s).AppendLine(value);
        //}

        //public static StringBuilder AppendFormat(this string s, string format, params object[] args)
        //{
        //    return StringBuilderPool.Get().AppendFormat(format, args);
        //}

        //public static string ToTrimEnd(this StringBuilder sb, params char[] trimChars)
        //{
        //    string s = string.Empty;

        //    if (trimChars == null)
        //    {
        //        s = sb.ToString().TrimEnd('\r', '\n'); // 三个或以上
        //    }
        //    else
        //    {
        //        s = sb.ToString().TrimEnd(trimChars);
        //    }
        //    StringBuilderPool.Release(sb);
            
        //    return s;
        //}
    }
}

