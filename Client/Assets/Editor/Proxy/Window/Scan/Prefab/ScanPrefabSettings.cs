// using System;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using System.Linq;
//
// namespace Ninth.Editor.Window
// {
//     public sealed partial class ScanPrefabSettings
//     {
//         private ScanPrefabMode ScanPrefabMode
//         {
//             get => WindowSOCore.Get<ScanSO>().ScanPrefabMode;
//             set => WindowSOCore.Get<ScanSO>().ScanPrefabMode = value;
//         }
//
//         private Dictionary<ScanPrefabMode, Action> cache;
//         private Dictionary<ScanPrefabMode, Action> Cache
//         {
//             get
//             {
//                 if (cache == null)
//                 {
//                     cache = new Dictionary<ScanPrefabMode, Action>();
//                     cache.Add(ScanPrefabMode.Image, new ScanPrefabImageSettings().OnDraw);
//                     cache.Add(ScanPrefabMode.Text, null);
//                 }
//                 return cache;
//             }
//         }
//
//         public void OnDraw()
//         {
//             string[] barMenu = Cache.Keys.Select(x => x.ToString()).ToArray();
//             ScanPrefabMode = (ScanPrefabMode)GUILayout.Toolbar((int)ScanPrefabMode, barMenu);
//             if (Cache.TryGetValue(ScanPrefabMode, out Action action))
//             {
//                 action?.Invoke();
//             }
//         }
//     }
// }