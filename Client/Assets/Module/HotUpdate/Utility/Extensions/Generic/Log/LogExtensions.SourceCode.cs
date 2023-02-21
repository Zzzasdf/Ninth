using System;
using System.Collections.Generic;
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
            }

            #region Frame
            public static T FrameLog<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameLog))
                {
                    Debug.LogFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> FrameLog<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameLog))
                {
                    Debug.LogFormat(FrameFormat(), MessageListFormat(message, format));
                }
                return message;
            }

            public static T FrameWarning<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameWarning))
                {
                    Debug.LogWarningFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> FrameWarning<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameWarning))
                {
                    Debug.LogWarningFormat(FrameFormat(), MessageListFormat(message, format));
                }
                return message;
            }

            public static T FrameError<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameError))
                {
                    Debug.LogErrorFormat(FrameFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> FrameError<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.FrameError))
                {
                    Debug.LogErrorFormat(FrameFormat(), MessageListFormat(message, format));
                }
                return message;
            }
            #endregion

            #region Game
            public static T Log<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Log))
                {
                    Debug.LogFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> Log<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Log))
                {
                    Debug.LogFormat(TimeFormat(), MessageListFormat(message, format));
                }
                return message;
            }

            public static T Warning<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Warning))
                {
                    Debug.LogWarningFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> Warning<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Warning))
                {
                    Debug.LogWarningFormat(TimeFormat(), MessageListFormat(message, format));
                }
                return message;
            }

            public static T Error<T>(T message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Error))
                {
                    Debug.LogErrorFormat(TimeFormat(), MessageFormat(message, format));
                }
                return message;
            }

            public static List<T> Error<T>(List<T> message, string format = null)
            {
                if (Validator.HasFlag(LogValidator.Error))
                {
                    Debug.LogErrorFormat(TimeFormat(), MessageListFormat(message, format));
                }
                return message;
            }
            #endregion


            private static string FrameFormat()
            {
                if (FramePrefix == null)
                {
                    FramePrefix = string.Join(' ', "Frame".AddColor(ColorDefine.FrameLog), "..");
                }
                return FramePrefix + TimeFormat();
            }

            private static string TimeFormat()
            {
                return string.Join(' ', DateTime.Now.ToString(), "{0}");
            }

            private static string MessageFormat<T>(T message, string format = null)
            {
                if (string.IsNullOrEmpty(format))
                {
                    return message.ToString();
                }
                else
                {
                    return string.Format(format, message.ToString());
                }
            }

            private static string MessageListFormat<T>(List<T> message, string format = null)
            {
                if (string.IsNullOrEmpty(format))
                {
                    return string.Join(',', message);
                }
                else
                {
                    return string.Format(format, string.Join(',', message));
                }
            }
        }
    }
}
