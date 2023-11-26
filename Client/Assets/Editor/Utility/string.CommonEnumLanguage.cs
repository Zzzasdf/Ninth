namespace Ninth.Editor
{
    public enum CommonLanguage
    {
        Browse,
        FrameBox,

        // ��� Build
        ExportTargetFolderSettings,
        BuildBundlesTargetFolderRoot,
        BuildPlayersTargetFolderRoot,
        SelectATargetFolderRootToExport,
        BuildTarget,
        BuildTargetGroup,
        MajorVersion,
        MinorVersion,
        RevisionNumber,
        ResetVersion,
        PleaseAdjustTheVersion,
        Export,

        // ��� Excel
        SearchTargetFolderSettings,
        SelectAFolderToSearch,
        Compile,
    }

    public static partial class StringLanguage
    {
        static void CommonEnumInit()
        {
            AddConvertibleTxtDefine(CommonLanguage.FrameBox, "frameBox", "frameBox"); // GUISkin��ʽ
            AddConvertibleTxtDefine(CommonLanguage.Browse, "���", "Browse");
            BuildInit();
            ExcelInit();

            void BuildInit()
            {
                AddConvertibleTxtDefine(CommonLanguage.ExportTargetFolderSettings, "ѡ�񵼳���Ŀ��Ŀ¼", "Export Target Folder Settings");
                AddConvertibleTxtDefine(CommonLanguage.BuildBundlesTargetFolderRoot, "ab�������Ŀ¼", "BundlesTargetFolderRoot");
                AddConvertibleTxtDefine(CommonLanguage.BuildPlayersTargetFolderRoot, "�ͻ��������Ŀ¼", "PlayersTargetFolderRoot");
                AddConvertibleTxtDefine(CommonLanguage.SelectATargetFolderRootToExport, "ѡ��һ��Ŀ¼����", "Select A Folder To Export");
                AddConvertibleTxtDefine(CommonLanguage.BuildTarget, "ab��������ƽ̨", "BuildTarget");
                AddConvertibleTxtDefine(CommonLanguage.BuildTargetGroup, "�ͻ��˹�����ƽ̨", "BuildTargetGroup");
                AddConvertibleTxtDefine(CommonLanguage.MajorVersion, "���汾", "MajorVersion");
                AddConvertibleTxtDefine(CommonLanguage.MinorVersion, "�ΰ汾", "MinorVersion");
                AddConvertibleTxtDefine(CommonLanguage.RevisionNumber, "�޶���", "RevisionNumber");
                AddConvertibleTxtDefine(CommonLanguage.ResetVersion, "��ԭ�汾", "ResetVersion");
                AddConvertibleTxtDefine(CommonLanguage.PleaseAdjustTheVersion, "���޸İ汾", "Adjust The Version, Please!!");
                AddConvertibleTxtDefine(CommonLanguage.Export, "����", "Export");
            }

            void ExcelInit()
            {
                AddConvertibleTxtDefine(CommonLanguage.SearchTargetFolderSettings, "ѡ��������Ŀ��Ŀ¼", "Search Target Folder Settings");
                AddConvertibleTxtDefine(CommonLanguage.SelectAFolderToSearch, "ѡ��һ��Ŀ¼����", "Select A Folder To Export Search");
                AddConvertibleTxtDefine(CommonLanguage.Compile, "����", "Compile");
            }
        }
    }
}