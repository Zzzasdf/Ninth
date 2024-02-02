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
            private static object lockObj = new object();
            private static LogValidator Validator;
            private static string FramePrefix;
            static LogSystem()
            {
                Validator = HotUpdateConfig.LogValidator;
                FramePrefix = string.Join(' ', "Frame".AddColor(ColorDefine.FrameLog), "..");
            }

            public static T Debug<T>(LogValidator logValidator, T message, string? format = null)
            {
                lock(lockObj)
                {
                    LogMessage(logValidator, MessageFormat(message, format));
                    return message;
                }

                string MessageFormat(T message, string? format = null)
                {
                    string result = message?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(format))
                    {
                        result = string.Format(format, result);
                    }
                    return result;
                }
            }

            public static IEnumerable<T> Debug<T>(LogValidator logValidator, IEnumerable<T> message, string? format = null)
            {
                lock(lockObj)
                {
                    LogMessage(logValidator, MessageIEnumerableFormat(message, format));
                    return message;
                }

                string MessageIEnumerableFormat<T>(IEnumerable<T> message, string? format = null)
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

            private static void LogMessage(LogValidator logValidator, string message)
            {
                if(!Validator.HasFlag(logValidator))
                {
                    return;
                }
                (Action<string, object[]>? func, Func<string>? format) group = logValidator switch
                {
                    LogValidator.FrameLog => (UnityEngine.Debug.LogFormat, FrameFormat),
                    LogValidator.FrameWarning => (UnityEngine.Debug.LogWarningFormat, FrameFormat),
                    LogValidator.FrameError => (UnityEngine.Debug.LogErrorFormat, FrameFormat),

                    LogValidator.Log => (UnityEngine.Debug.LogFormat, TimeFormat),
                    LogValidator.Warning => (UnityEngine.Debug.LogWarningFormat, TimeFormat),
                    LogValidator.Error => (UnityEngine.Debug.LogErrorFormat, TimeFormat),

                    _ => (null, null)
                };
                if(group.func == null || group.format == null)
                {
                    return;
                }
                group.func.Invoke(group.format.Invoke(), new object[] { message });

                string FrameFormat()
                {
                    return FramePrefix + TimeFormat();
                }

                string TimeFormat()
                {
                    int currentManagedThreadId = Environment.CurrentManagedThreadId;
                    string threadIdStr = $"ThreadId: {currentManagedThreadId}";
                    if(currentManagedThreadId != 1)
                    {
                        threadIdStr = threadIdStr.AddColor(ColorDefine.NonMainThread);
                    }
                    return string.Join(' ', DateTime.Now.ToString(), threadIdStr, "\n{0}");
                }
            }
        }
    }
}
