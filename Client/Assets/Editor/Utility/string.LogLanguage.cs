namespace Ninth.Editor
{
    public enum Log
    {
        Exporting,
        ExportCompleted,
        FuncIsNull,
    }

    public enum LogFormat1
    {
        PathNotIncludedAppointFolder,
    }

    public enum LogFormat2
    { 
    
    }

    public static partial class StringLanguage
    {
        static void LogEnumInit()
        {
            LogInit();
            LogFormat1Init();
            LogFormat2Init();

            void LogInit()
            {
                AddConvertibleTxtDefine(Log.Exporting, "������ ..", "Exporting ..");
                AddConvertibleTxtDefine(Log.ExportCompleted, "������� !!", "Export Completed !!");
                AddConvertibleTxtDefine(Log.FuncIsNull, " ����Ϊ�� !!", "Func Is Null !!");
            }

            void LogFormat1Init()
            {
                AddConvertibleTxtDefine(LogFormat1.PathNotIncludedAppointFolder, "��·��δ����ָ����Ŀ¼{0}", "Path Not Included Appoint Folder {0}");
            }

            void LogFormat2Init()
            {

            }
        }
    }
}
