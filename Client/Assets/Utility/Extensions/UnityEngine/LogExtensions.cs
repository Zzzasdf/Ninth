using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static partial class LogExtensions
    {
#region Frame
        public static T FrameLog<T>(this T obj, string? format = null)
            => Debug(LogValidator.FrameLog, obj, format);
        public static IEnumerable<T> FrameLog<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.FrameLog, obj, format);

        public static T FrameWarning<T>(this T obj, string? format = null)
            => Debug(LogValidator.FrameWarning, obj, format);
        public static IEnumerable<T> FrameWarning<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.FrameWarning, obj, format);

        public static T FrameError<T>(this T obj, string? format = null)
            => Debug(LogValidator.FrameError, obj, format);
        public static IEnumerable<T> FrameError<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.FrameError, obj, format);
#endregion

#region Game
        public static T Log<T>(this T obj, string? format = null)
            => Debug(LogValidator.Log, obj, format);
        public static IEnumerable<T> Log<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.Log, obj, format);
            
        public static T[] Log<T>(this T[] obj, string? format = null)
            => ((obj as IEnumerable<T>).Log(format) as T[])!;
        public static List<T> Log<T>(this List<T> obj, string? format = null)
             => ((obj as IEnumerable<T>).Log(format) as List<T>)!;
        public static IOrderedEnumerable<T> Log<T>(this IOrderedEnumerable<T> obj, string? format = null)
            => ((obj as IEnumerable<T>).Log(format) as IOrderedEnumerable<T>)!;
        public static ParallelQuery<T> Log<T>(this ParallelQuery<T> obj, string? format = null)
            => ((obj as IEnumerable<T>).Log(format) as ParallelQuery<T>)!;
        
        public static T Warning<T>(this T obj, string? format = null)
            => Debug(LogValidator.Warning, obj, format);
        public static IEnumerable<T> Warning<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.Warning, obj, format);

        public static T Error<T>(this T obj, string? format = null)
            => Debug(LogValidator.Error, obj, format);
        public static IEnumerable<T> Error<T>(this IEnumerable<T> obj, string? format = null)
            => Debug(LogValidator.Error, obj, format);
#endregion
    }
}

