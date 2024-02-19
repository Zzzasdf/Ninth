// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Ninth.Editor.Window
// {
//     public sealed class ScanPrefabImageSettings : ScanPrefabComponentSettings<ScanPrefabImageMode, Image>
//     {
//         protected override void ClassifiesAssembler()
//         {
//             AssembleClassify(ScanPrefabImageMode.LackMaterial, LackMaterial, "缺少材质", null);
//             AssembleClassify(ScanPrefabImageMode.WhiteColor, WhiteColor, "白色", WhiteColorHandle);
//         }
//
//         // 是否缺少材质
//         private bool LackMaterial(Image image)
//         {
//             return image.material.name == "Default UI Material";
//         }
//
//         // 是否是白色
//         private bool WhiteColor(Image image)
//         {
//             return image.color == Color.white;
//         }
//
//         private void WhiteColorHandle(Image image)
//         {
//             image.color = Color.red;
//         }
//
//         // 实现抽象方法
//         protected override string GetPathDirectoryRoot()
//         {
//             return WindowSOCore.Get<ScanSO>().PrefabImageScanPathDirectoryRoot;
//         }
//
//         protected override void SetPathDirectoryRoot(string value)
//         {
//             WindowSOCore.Get<ScanSO>().PrefabImageScanPathDirectoryRoot = value;
//         }
//
//         protected override int EnumToInt(ScanPrefabImageMode tEnum)
//         {
//             return (int)tEnum;
//         }
//
//         protected override ScanPrefabImageMode IntToEnum(int value)
//         {
//             return (ScanPrefabImageMode)value;
//         }
//     }
//
//     public enum ScanPrefabImageMode
//     {
//         LackMaterial,
//         WhiteColor,
//     }
// }