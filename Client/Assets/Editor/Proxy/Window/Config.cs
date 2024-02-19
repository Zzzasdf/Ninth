// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using UnityEngine;
// using VContainer;
//
// namespace Ninth.Editor.Window
// {
//     public class Config: IConfig
//     {
//         private readonly ISO so;
//         
//         private readonly BaseSubscribe<Tab, Type?> tabSubscribe;
//
//         [Inject]
//         public Config(ISO so)
//         {
//             this.so = so;
//             
//             tabSubscribe = new BaseSubscribe<Tab, Type?>
//             {
//                 [Tab.Build] = typeof(IBuildProxy),
//                 [Tab.Excel] = typeof(IExcelProxy),
//                 [Tab.Scan] = typeof(IScanProxy),
//                 [Tab.Other] = typeof(IOtherProxy),
//             };
//         }
//
//         Tab IConfig.CurrentTab
//         {
//             get => so.Tab;
//             set => so.Tab = value;
//         }
//
//         Type? IConfig.Get(Tab tab)
//         {
//             return tabSubscribe.Get(tab);
//         }
//
//         void IConfig.Set(Tab tab)
//         {
//             so.Tab = tab;
//         }
//
//         ReadOnlyDictionary<Tab, Type?>.KeyCollection IConfig.Keys => tabSubscribe.Keys;
//         ReadOnlyDictionary<Tab, Type?>.ValueCollection IConfig.Values => tabSubscribe.Values;
//
//         void IConfig.Save()
//         {
//             so.Save();
//         }
//     }
// }
