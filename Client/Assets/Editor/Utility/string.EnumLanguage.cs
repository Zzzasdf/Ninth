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
                AddConvertibleTxtDefine(NinthWindowTab.Build, "���", "Build");
                AddConvertibleTxtDefine(NinthWindowTab.Excel, "���", "Excel");
                AddConvertibleTxtDefine(NinthWindowTab.Scan, "ɨ��", "Scan");
                AddConvertibleTxtDefine(NinthWindowTab.Review, "??", "Review");
                AddConvertibleTxtDefine(NinthWindowTab.Other, "����", "Other");

                AddConvertibleTxtDefine(BuildSettingsMode.Bundle, "����ab��", "Bundle");
                AddConvertibleTxtDefine(BuildSettingsMode.Player, "�����ͻ���", "Player");

                AddConvertibleTxtDefine(BuildBundleMode.HotUpdateBundles, "�ȸ�ab��", "HotUpdateBundles");
                AddConvertibleTxtDefine(BuildBundleMode.AllBundles, "����ab��", "AllBundles");

                AddConvertibleTxtDefine(BuildExportCopyFolderMode.StreamingAssets, "{ ����ab�� => StreamingAssets }", "{ AllBundles => StreamingAssets }");
                AddConvertibleTxtDefine(BuildExportCopyFolderMode.Remote, "{ ����ab�� => StreamingAssets }\n{ �ȸ�ab�� => Զ��·�� }", "{ LocalBundles => StreamingAssets }\n{ HotUpdateBundles => RemoteFolder }");

                AddConvertibleTxtDefine(ActiveTargetMode.ActiveTarget, "Unity��ǰʹ��ƽ̨", "ActiveTarget");
                AddConvertibleTxtDefine(ActiveTargetMode.InactiveTarget, "ѡ��ƽ̨", "InactiveTarget");
            }

            void ExcelInit()
            {
                AddConvertibleTxtDefine(ExcelMode.Encode, "����", "Encode");
                AddConvertibleTxtDefine(ExcelMode.Search, "����", "Search");
            }
        }
    }
}