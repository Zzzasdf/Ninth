using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public static partial class LogExtensions
    {
        public static T FrameLog<T>(this T obj, Object tag = null)
        {
            return LogSystem.FrameLog(obj, tag);
        }

        public static List<T> FrameLog<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.FrameLog(obj, tag);
        }

        public static T FrameWarning<T>(this T obj, Object tag = null)
        {
            return LogSystem.FrameWarning(obj, tag);
        }

        public static List<T> FrameWarning<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.FrameWarning(obj, tag);
        }

        public static T FrameError<T>(this T obj, Object tag = null)
        {
            return LogSystem.FrameError(obj, tag);
        }

        public static List<T> FrameError<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.FrameError(obj, tag);
        }

        public static T Log<T>(this T obj, Object tag = null)
        {
            return LogSystem.Log(obj, tag);
        }

        public static List<T> Log<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.Log(obj, tag);
        }

        public static T Warning<T>(this T obj, Object tag = null)
        {
            return LogSystem.Warning(obj, tag);
        }

        public static List<T> Warning<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.Warning(obj, tag);
        }

        public static T Error<T>(this T obj, Object tag = null)
        {
            return LogSystem.Error(obj, tag);
        }

        public static List<T> Error<T>(this List<T> obj, Object tag = null)
        {
            return LogSystem.Error(obj, tag);
        }
    }
}

