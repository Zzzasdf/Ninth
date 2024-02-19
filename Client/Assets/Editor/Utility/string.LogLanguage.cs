// namespace Ninth.Editor
// {
//     public enum Log
//     {
//         Exporting,
//         ExportCompleted,
//         FuncIsNull,
//     }
//
//     public enum LogFormat1
//     {
//         PathNotIncludedAppointFolder,
//     }
//
//     public enum LogFormat2
//     { 
//     
//     }
//
//     public static partial class StringLanguage
//     {
//         static void LogEnumInit()
//         {
//             LogInit();
//             LogFormat1Init();
//             LogFormat2Init();
//
//             void LogInit()
//             {
//                 AddConvertibleTxtDefine(Log.Exporting, "导出中 ..", "Exporting ..");
//                 AddConvertibleTxtDefine(Log.ExportCompleted, "导出完成 !!", "Export Completed !!");
//                 AddConvertibleTxtDefine(Log.FuncIsNull, " 方法为空 !!", "Func Is Null !!");
//             }
//
//             void LogFormat1Init()
//             {
//                 AddConvertibleTxtDefine(LogFormat1.PathNotIncludedAppointFolder, "该路径未包含指定的目录{0}", "Path Not Included Appoint Folder {0}");
//             }
//
//             void LogFormat2Init()
//             {
//
//             }
//         }
//     }
// }
