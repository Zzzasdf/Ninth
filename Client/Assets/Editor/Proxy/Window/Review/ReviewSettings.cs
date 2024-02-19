// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.Linq;
//
// namespace Ninth.Editor.Window
// {
//     public class ReviewSettings
//     {
//         private Dictionary<ReviewMode, Action> cache;
//         private Dictionary<ReviewMode, Action> Cache
//         {
//             get
//             {
//                 if (cache == null)
//                 {
//                     cache = new Dictionary<ReviewMode, Action>();
//                     cache.Add(ReviewMode.PrefabAndScrpit, new ReviewPrefabScriptSettings().OnDraw);
//                     cache.Add(ReviewMode.Others, null);
//                 }
//                 return cache;
//             }
//         }
//
//         private ReviewMode ReviewMode
//         {
//             get => WindowSOCore.Get<WindowReviewConfig>().ReviewMode;
//             set => WindowSOCore.Get<WindowReviewConfig>().ReviewMode = value;
//         }
//
//         public void OnGUI()
//         {
//             string[] barMenu = Cache.Keys.Select(x => x.ToString()).ToArray();
//             ReviewMode = (ReviewMode)GUILayout.Toolbar((int)ReviewMode, barMenu);
//             if (Cache.TryGetValue(ReviewMode, out Action action))
//             {
//                 action?.Invoke();
//             }
//         }
//     }
// }