using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.Editor
{
    public sealed class ScanPrefabImageSettings : ScanPrefabComponentSettings<ScanPrefabImageMode, Image>
    {
        protected override void ClassifiesAssembler()
        {
            AssembleClassify(ScanPrefabImageMode.LackMaterial, LackMaterial, "ȱ�ٲ���", null);
            AssembleClassify(ScanPrefabImageMode.WhiteColor, WhiteColor, "��ɫ", WhiteColorHandle);
        }

        // �Ƿ�ȱ�ٲ���
        private bool LackMaterial(Image image)
        {
            return image.material.name == "Default UI Material";
        }

        // �Ƿ��ǰ�ɫ
        private bool WhiteColor(Image image)
        {
            return image.color == Color.white;
        }

        private void WhiteColorHandle(Image image)
        {
            image.color = Color.red;
        }

        // ʵ�ֳ��󷽷�
        protected override string GetPathDirectoryRoot()
        {
            return EditorSOCore.GetScanConfig().PrefabImageScanPathDirectoryRoot;
        }

        protected override void SetPathDirectoryRoot(string value)
        {
            EditorSOCore.GetScanConfig().PrefabImageScanPathDirectoryRoot = value;
        }

        protected override int EnumToInt(ScanPrefabImageMode tEnum)
        {
            return (int)tEnum;
        }

        protected override ScanPrefabImageMode IntToEnum(int value)
        {
            return (ScanPrefabImageMode)value;
        }
    }

    public enum ScanPrefabImageMode
    {
        LackMaterial,
        WhiteColor,
    }
}