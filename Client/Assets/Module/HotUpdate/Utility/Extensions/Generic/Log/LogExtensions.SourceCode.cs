using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public static partial class LogExtensions
    {
        private static class LogSystem
        {
            private static LogValidator Validator;

            private static string FramePrefix;

            static LogSystem()
            {
                Validator = HotUpdateConfig.LogValidator;
                FramePrefix = string.Join(' ', "Frame".AddColor(ColorDefine.FrameLog), "..");
            }

#region Frame
            public static T FrameLog<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameLog))
                {
                    Debug.LogFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> FrameLog<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameLog))
                {
                    Debug.LogFormat(FrameFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }

            public static T FrameWarning<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameWarning))
                {
                    Debug.LogWarningFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> FrameWarning<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameWarning))
                {
                    Debug.LogWarningFormat(FrameFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }

            public static T FrameError<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameError))
                {
                    Debug.LogErrorFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> FrameError<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.FrameError))
                {
                    Debug.LogErrorFormat(FrameFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }
#endregion

#region Game
            public static T Log<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Log))
                {
                    Debug.LogFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> Log<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Log))
                {
                    Debug.LogFormat(TimeFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }

            public static T Warning<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Warning))
                {
                    Debug.LogWarningFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> Warning<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Warning))
                {
                    Debug.LogWarningFormat(TimeFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }

            public static T Error<T>(T message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Error))
                {
                    Debug.LogErrorFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static IEnumerable<T> Error<T>(IEnumerable<T> message, string? format)
            {
                if (Validator.HasFlag(LogValidator.Error))
                {
                    Debug.LogErrorFormat(TimeFormat(), MessageIEnumerableFormat(message, format));
                }
                return message;
            }
#endregion


            private static string FrameFormat()
            {
                return FramePrefix + TimeFormat();
            }

            private static string TimeFormat()
            {
                int currentManagedThreadId = Environment.CurrentManagedThreadId;
                string threadIdStr = $"ThreadId: {currentManagedThreadId}";
                if(currentManagedThreadId != 1)
                {
                    threadIdStr = threadIdStr.AddColor(ColorDefine.NonMainThread);
                }
                return string.Join(' ', DateTime.Now.ToString(), threadIdStr, "\n{0}");
            }

            private static string MessageFormat<T>(T message, string? format)
            {
                string result = message?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(format))
                {
                    result = string.Format(format, result);
                }
                return result;
            }

            private static string MessageIEnumerableFormat<T>(IEnumerable<T> message, string? format)
            {
                StringBuilder sb = new StringBuilder().AppendLine();
                int index = 0;
                foreach (var item in message)
                {
                    sb.AppendLine($" [{index}] => [{item}]");
                    index++;
                }
                string result = sb.ToString().TrimEnd('\n', '\r');
                if (!string.IsNullOrEmpty(format))
                    result = string.Format(format, result);
                return $"{message} => {result}";
            }
        }
    }
}
