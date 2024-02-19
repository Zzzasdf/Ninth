// namespace Ninth.Editor
// {
//     public enum CommonLanguage
//     {
//         Browse,
//         FrameBox,
//
//         // 打包 Build
//         ExportTargetFolderSettings,
//         BuildBundlesTargetFolderRoot,
//         BuildPlayersTargetFolderRoot,
//         SelectATargetFolderRootToExport,
//         CopyOperate,
//         BuildTarget,
//         BuildTargetGroup,
//         MajorVersion,
//         MinorVersion,
//         VersionRevisionNumber,
//         RevisionNumber,
//         ResetVersion,
//         PleaseAdjustTheVersion,
//         Export,
//
//         // 表格 Excel
//         SearchTargetFolderSettings,
//         SelectAFolderToSearch,
//         Compile,
//     }
//
//     public static partial class StringLanguage
//     {
//         static void CommonEnumInit()
//         {
//             AddConvertibleTxtDefine(CommonLanguage.FrameBox, "frameBox", "frameBox"); // GUISkin格式
//             AddConvertibleTxtDefine(CommonLanguage.Browse, "浏览", "Browse");
//             BuildInit();
//             ExcelInit();
//
//             void BuildInit()
//             {
//                 AddConvertibleTxtDefine(CommonLanguage.ExportTargetFolderSettings, "选择导出的目标目录", "Export Target Folder Settings");
//                 AddConvertibleTxtDefine(CommonLanguage.BuildBundlesTargetFolderRoot, "ab包输出的目录", "BundlesTargetFolderRoot");
//                 AddConvertibleTxtDefine(CommonLanguage.BuildPlayersTargetFolderRoot, "客户端输出的目录", "PlayersTargetFolderRoot");
//                 AddConvertibleTxtDefine(CommonLanguage.SelectATargetFolderRootToExport, "选择一个目录导出", "Select A Folder To Export");
//                 AddConvertibleTxtDefine(CommonLanguage.CopyOperate, "拷贝\n操作", "Copy\nOperate");
//                 AddConvertibleTxtDefine(CommonLanguage.BuildTarget, "ab包构建的平台", "BuildTarget");
//                 AddConvertibleTxtDefine(CommonLanguage.BuildTargetGroup, "客户端构建的平台", "BuildTargetGroup");
//                 AddConvertibleTxtDefine(CommonLanguage.MajorVersion, "主版本", "MajorVersion");
//                 AddConvertibleTxtDefine(CommonLanguage.MinorVersion, "次版本", "MinorVersion");
//                 AddConvertibleTxtDefine(CommonLanguage.VersionRevisionNumber, "版本修订号", "VersionRevisionNumber");
//                 AddConvertibleTxtDefine(CommonLanguage.RevisionNumber, "修订号", "RevisionNumber");
//                 AddConvertibleTxtDefine(CommonLanguage.ResetVersion, "还原版本", "ResetVersion");
//                 AddConvertibleTxtDefine(CommonLanguage.PleaseAdjustTheVersion, "请修改版本", "Adjust The Version, Please!!");
//                 AddConvertibleTxtDefine(CommonLanguage.Export, "导出", "Export");
//             }
//
//             void ExcelInit()
//             {
//                 AddConvertibleTxtDefine(CommonLanguage.SearchTargetFolderSettings, "选择搜索的目标目录", "Search Target Folder Settings");
//                 AddConvertibleTxtDefine(CommonLanguage.SelectAFolderToSearch, "选择一个目录搜索", "Select A Folder To Export Search");
//                 AddConvertibleTxtDefine(CommonLanguage.Compile, "编译", "Compile");
//             }
//         }
//     }
// }