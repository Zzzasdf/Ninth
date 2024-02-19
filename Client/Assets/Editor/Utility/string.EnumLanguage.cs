// namespace Ninth.Editor
// {
//     public static partial class StringLanguage
//     {
//         static void EnumInit()
//         {
//             BuildInit();
//             ExcelInit();
//
//             void BuildInit()
//             {
//                 AddConvertibleTxtDefine(EditorWindowTab.Build, "打包", "Build");
//                 AddConvertibleTxtDefine(EditorWindowTab.Excel, "表格", "Excel");
//                 AddConvertibleTxtDefine(EditorWindowTab.Scan, "扫描", "Scan");
//                 AddConvertibleTxtDefine(EditorWindowTab.Review, "??", "Review");
//                 AddConvertibleTxtDefine(EditorWindowTab.Other, "其他", "Other");
//
//                 AddConvertibleTxtDefine(BuildSettingsMode.Bundle, "构建ab包", "Bundle");
//                 AddConvertibleTxtDefine(BuildSettingsMode.Player, "构建客户端", "Player");
//
//                 AddConvertibleTxtDefine(BuildBundleMode.HotUpdateBundles, "热更ab包", "HotUpdateBundles");
//                 AddConvertibleTxtDefine(BuildBundleMode.AllBundles, "所有ab包", "AllBundles");
//
//                 AddConvertibleTxtDefine(BuildExportCopyFolderMode.StreamingAssets, "{ 所有ab包 => StreamingAssets }", "{ AllBundles => StreamingAssets }");
//                 AddConvertibleTxtDefine(BuildExportCopyFolderMode.Remote, "{ 本地ab包 => StreamingAssets }\n{ 热更ab包 => 远端路径 }", "{ LocalBundles => StreamingAssets }\n{ HotUpdateBundles => RemoteFolder }");
//
//                 AddConvertibleTxtDefine(ActiveTargetMode.ActiveTarget, "Unity当前使用平台", "ActiveTarget");
//                 AddConvertibleTxtDefine(ActiveTargetMode.InactiveTarget, "选择平台", "InactiveTarget");
//             }
//
//             void ExcelInit()
//             {
//                 AddConvertibleTxtDefine(ExcelMode.Encode, "编码", "Encode");
//                 AddConvertibleTxtDefine(ExcelMode.Search, "搜索", "Search");
//             }
//         }
//     }
// }