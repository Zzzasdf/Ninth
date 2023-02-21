using System.Collections.Generic;
using System;

namespace Ninth.HotUpdate
{
    public static partial class LogExtensions
    {
        public static T FrameLog<T>(this T obj, string format = null)
        {
            return LogSystem.FrameLog(obj, format);
        }

        public static List<T> FrameLog<T>(this List<T> obj, string format = null)
        {
            return LogSystem.FrameLog(obj, format);
        }

        public static T FrameWarning<T>(this T obj, string format = null)
        {
            return LogSystem.FrameWarning(obj, format);
        }

        public static List<T> FrameWarning<T>(this List<T> obj, string format = null)
        {
            return LogSystem.FrameWarning(obj, format);
        }

        public static T FrameError<T>(this T obj, string format = null)
        {
            return LogSystem.FrameError(obj, format);
        }

        public static List<T> FrameError<T>(this List<T> obj, string format = null)
        {
            return LogSystem.FrameError(obj, format);
        }

        public static T Log<T>(this T obj, string format = null)
        {
            return LogSystem.Log(obj, format);
        }

        public static List<T> Log<T>(this List<T> obj, string format = null)
        {
            return LogSystem.Log(obj, format);
        }

        public static T Warning<T>(this T obj, string format = null)
        {
            return LogSystem.Warning(obj, format);
        }

        public static List<T> Warning<T>(this List<T> obj, string format = null)
        {
            return LogSystem.Warning(obj, format);
        }

        public static T Error<T>(this T obj, string format = null)
        {
            return LogSystem.Error(obj, format);
        }

        public static List<T> Error<T>(this List<T> obj, string format = null)
        {
            return LogSystem.Error(obj, format);
        }
    }
}

