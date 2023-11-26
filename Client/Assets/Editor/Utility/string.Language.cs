using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Ninth.Editor
{
    public enum Language
    {
        Chinese,
        English,
    }

    public static partial class StringLanguage
    {
        private static Language language = Language.English;
        private static ReadOnlyDictionary<Language, string> undefinedDic = new(new Dictionary<Language, string>()
        {
            { Language.Chinese, "未定义" },
            { Language.English, "undefined" },
        });
        
        static StringLanguage()
        { 
            EnumInit();
            CommonEnumInit();
            LogEnumInit();
        }

        private static Dictionary<Enum, string[]> convertibleTxtDefineDic;
        private static Dictionary<string, string[]> EnglishConvertibleTxtDefineDic;
        static void AddConvertibleTxtDefine(Enum enumType, params string[] languageTxts)
        {
            if (convertibleTxtDefineDic == null)
            {
                convertibleTxtDefineDic = new Dictionary<Enum, string[]>();
            }
            convertibleTxtDefineDic.Add(enumType, languageTxts);
            if (EnglishConvertibleTxtDefineDic == null)
            {
                EnglishConvertibleTxtDefineDic = new Dictionary<string, string[]>();
            }
            if (languageTxts?.Length > 0)
            {
                EnglishConvertibleTxtDefineDic.Add(languageTxts[0], languageTxts);
            }
        }

        // 转换成当前的语言
        public static string[] ToCurrLanguage<T>(this T[] enumTypes) where T: Enum
        {
            if(enumTypes == null || enumTypes.Length == 0)
            {
                return null;
            }
            string[] result = new string[enumTypes.Length];
            for(int i = 0; i < enumTypes.Length; i++)
            {
                result[i] = ToCurrLanguage(enumTypes[i]);
            }
            return result;
        }
        public static string ToCurrLanguage(this Enum enumType)
        {
            return ToAppointLanguage(enumType, language);
        }

        public static string ToCurrLanguage(this string txt)
        {
            return ToAppointLanguage(txt, language);
        }

        // 转换成指定的语言
        public static string ToAppointLanguage(this Enum enumType, Language language)
        {
            if (!convertibleTxtDefineDic.TryGetValue(enumType, out string[] value))
            {
                return string.Format("{0}({1})", enumType.ToString(), undefinedDic[language]);
            }
            int languageIndex = (int)language;
            if(languageIndex >= value.Length)
            {
                if(value.Length == 0)
                {
                    return string.Format("{0}({1})", enumType.ToString(), undefinedDic[language]);
                }
                return string.Format("{0}({1})", value[0], undefinedDic[language]);
            }
            return value[languageIndex];
        }

        public static string ToAppointLanguage(this string txt, Language language)
        {
            if (!EnglishConvertibleTxtDefineDic.TryGetValue(txt, out string[] value))
            {
                return undefinedDic[language];
            }
            int languageIndex = (int)language;
            if (languageIndex >= value.Length)
            {
                return undefinedDic[language];
            }
            return value[languageIndex];
        }
    }
}

