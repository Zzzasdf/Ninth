using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading;

namespace Ninth.HotUpdate
{
    public static partial class LogExtensions
    {
        private static class LogSystem
        {
            private static LogValidator Validator;
            private static string FramePrefix;
            private static ConcurrentQueue<LogMessage> logQueue;
            private static void LogQueueEnqueue(LogValidator validator, string message)
            {
                if (!Validator.HasFlag(validator))
                {
                    return;
                }
                LogMessage logMessage = new LogMessage(validator, message);
                logQueue.Enqueue(logMessage);
            }

            static LogSystem()
            {
                Validator = HotUpdateConfig.LogValidator;
                FramePrefix = string.Join(' ', "Frame".AddColor(ColorDefine.FrameLog), "..");

                logQueue = new();
                Thread logThread = new Thread(LogQueueDequeue);
                logThread.Start();

                void LogQueueDequeue()
                {
                    while(true)
                    {
                        if(logQueue.TryDequeue(out var result) && result == null)
                        {    
                            (LogValidator logValidator, string message) = result!;
                            (Action<string, object[]>? func, Func<string>? format) group = logValidator switch
                            {
                                LogValidator.FrameLog => (Debug.LogFormat, FrameFormat),
                                LogValidator.FrameWarning => (Debug.LogWarningFormat, FrameFormat),
                                LogValidator.FrameError => (Debug.LogErrorFormat, FrameFormat),

                                LogValidator.Log => (Debug.LogFormat, TimeFormat),
                                LogValidator.Warning => (Debug.LogWarningFormat, TimeFormat),
                                LogValidator.Error => (Debug.LogErrorFormat, TimeFormat),

                                _ => (null, null)
                            };
                            if(group.func == null || group.format == null)
                            {
                                continue;
                            }
                            group.func.Invoke(group.format.Invoke(), new object[]{ message });
                        }
                        Thread.Sleep(1);
                    }

                    string FrameFormat()
                    {
                        return FramePrefix + TimeFormat();
                    }
                    string TimeFormat()
                    {
                        int currentManagedThreadId = Environment.CurrentManagedThreadId;
                        string threadIdStr = $"ThreadId: {currentManagedThreadId}";
                        if (currentManagedThreadId != 1)
                        {
                            threadIdStr = threadIdStr.AddColor(ColorDefine.NonMainThread);
                        }
                        return string.Join(' ', DateTime.Now.ToString(), threadIdStr, "\n{0}");
                    }
                }
            }

#region Frame
            public static T FrameLog<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameLog, MessageFormat(message, format));
                return message;
            } 

            public static IEnumerable<T> FrameLog<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameLog, MessageIEnumerableFormat(message, format));
                return message;
            } 

            public static T FrameWarning<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameWarning, MessageFormat(message, format));
                return message;
            }

            public static IEnumerable<T> FrameWarning<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameWarning, MessageIEnumerableFormat(message, format));
                return message;
            }

            public static T FrameError<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameError, MessageFormat(message, format));
                return message;
            }

            public static IEnumerable<T> FrameError<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.FrameError, MessageIEnumerableFormat(message, format));
                return message;
            }
#endregion

#region Game
            public static T Log<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.Log, MessageFormat(message, format));
                return message;
            }

            public static IEnumerable<T> Log<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.Log, MessageIEnumerableFormat(message, format));
                return message;
            }

            public static T Warning<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.Warning, MessageFormat(message, format));
                return message;
            }

            public static IEnumerable<T> Warning<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.Warning, MessageIEnumerableFormat(message, format));
                return message;
            }

            public static T Error<T>(T message, string? format)
            {
                LogQueueEnqueue(LogValidator.Error, MessageFormat(message, format));
                return message;
            }

            public static IEnumerable<T> Error<T>(IEnumerable<T> message, string? format)
            {
                LogQueueEnqueue(LogValidator.Error, MessageIEnumerableFormat(message, format));
                return message;
            }
#endregion

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

            internal class LogMessage
            {
                private LogValidator validator;
                private string message;
                public LogMessage(LogValidator validator, string message)
                {
                    this.validator = validator;
                    this.message = message;
                }

                public void Deconstruct(out LogValidator validator, out string message)
                {
                    validator = this.validator;
                    message = this.message;
                }

                // public LogValidator validator WholeMessage()
                // {
                //     string? format = validator switch
                //     {
                //         >= LogValidator.FrameLog and  <= LogValidator.FrameError  => FrameFormat(),
                //         >= LogValidator.Log and <= LogValidator.Error => TimeFormat(),
                //         _ => null
                //     };
                //     if(format == null)
                //     {
                //         return null;
                //     }
                //     return string.Format(format, message);
                // }
            }
        }
    }
}
