namespace Ninth.Editor
{
    public static partial class StringLanguage
    {
        static void EnumInit()
        {
            BuildInit();
            ExcelInit();

            void BuildInit()
            {
                AddConvertibleTxtDefine(NinthWindowTab.Build, "打包", "Build");
                AddConvertibleTxtDefine(NinthWindowTab.Excel, "表格", "Excel");
                AddConvertibleTxtDefine(NinthWindowTab.Scan, "扫描", "Scan");
                AddConvertibleTxtDefine(NinthWindowTab.Review, "??", "Review");
                AddConvertibleTxtDefine(NinthWindowTab.Other, "其他", "Other");

                AddConvertibleTxtDefine(BuildSettingsMode.Bundle, "构建ab包", "Bundle");
                AddConvertibleTxtDefine(BuildSettingsMode.Player, "构建客户端", "Player");

                AddConvertibleTxtDefine(BuildBundleMode.HotUpdateBundles, "热更ab包", "HotUpdateBundles");
                AddConvertibleTxtDefine(BuildBundleMode.AllBundles, "所有ab包", "AllBundles");

                AddConvertibleTxtDefine(BuildPlayerMode.InoperationBundle, "重打本地ab包", "InoperationBundle");
                AddConvertibleTxtDefine(BuildPlayerMode.RepackageAllBundle, "重打所有ab包", "RepackageAllBundle");

                AddConvertibleTxtDefine(BuildExportCopyFolderMode.StreamingAssets, "拷贝ab包到StreamingAssets", "BundlesCopyToStreamingAssets");
                AddConvertibleTxtDefine(BuildExportCopyFolderMode.Remote, "拷贝ab包到远端路径", "BundlesCopyToRemoteDir");

                AddConvertibleTxtDefine(ActiveTargetMode.ActiveTarget, "Unity当前使用平台", "ActiveTarget");
                AddConvertibleTxtDefine(ActiveTargetMode.InactiveTarget, "选择平台", "InactiveTarget");
            }

            void ExcelInit()
            {
                AddConvertibleTxtDefine(ExcelMode.Encode, "编码", "Encode");
                AddConvertibleTxtDefine(ExcelMode.Search, "搜索", "Search");
            }
        }
    }
}