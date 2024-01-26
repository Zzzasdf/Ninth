using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Ninth.HotUpdate
{
    public static partial class LogExtensions
    {
        public static T FrameLog<T>(this T obj, string? format = null)
            => LogSystem.FrameLog(obj, format);

        public static IEnumerable<T> FrameLog<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.FrameLog(obj, format);

        public static T FrameWarning<T>(this T obj, string? format = null)
            => LogSystem.FrameWarning(obj, format);

        public static IEnumerable<T> FrameWarning<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.FrameWarning(obj, format);

        public static T FrameError<T>(this T obj, string? format = null)
            => LogSystem.FrameError(obj, format);

        public static IEnumerable<T> FrameError<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.FrameError(obj, format);

        public static T Log<T>(this T obj, string? format = null)
            => LogSystem.Log(obj, format);

        public static T[] Log<T>(this T[] obj, string? format = null)
            => LogSystem.Log<T>(obj, format).ToArray();
        public static List<T> Log<T>(this List<T> obj, string? format = null)
            => LogSystem.Log<T>(obj, format).ToList();
        public static IOrderedEnumerable<T> Log<T>(this IOrderedEnumerable<T> obj, string? format = null)
            => (IOrderedEnumerable<T>)LogSystem.Log<T>(obj, format);
        public static ParallelQuery<T> Log<T>(this ParallelQuery<T> obj, string? format = null)
            => (ParallelQuery<T>)LogSystem.Log<T>(obj, format);
        public static IEnumerable<T> Log<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.Log(obj, format);

        public static T Warning<T>(this T obj, string? format = null)
            => LogSystem.Warning(obj, format);

        public static IEnumerable<T> Warning<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.Warning(obj, format);

        public static T Error<T>(this T obj, string? format = null)
            => LogSystem.Error(obj, format);

        public static IEnumerable<T> Error<T>(this IEnumerable<T> obj, string? format = null)
            => LogSystem.Error(obj, format);
    }
}

