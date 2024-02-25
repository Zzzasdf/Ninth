// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Cysharp.Threading.Tasks;
// using Ninth.HotUpdate;
// using UnityEngine;
//
// namespace Ninth.Utility
// {
//     public class IndexSearchDictionary<TKey, TValue>
//     {
//         private readonly Dictionary<TKey, TValue> cache = new();
//         public int Count => cache.Count;
//
//         private TKey[] Keys;
//         private TValue[] Values;
//         
//         public string[] KeysString { get; private set; }
//         public int[] KeysIndex { get; private set; }
//
//         public TValue this[TKey key]
//         {
//             get => cache[key];
//             set => cache[key] = value;
//         }
//
//         public IndexSearchDictionary<TKey, TValue> Build()
//         {
//             Keys = cache.Keys.ToArray();
//             KeysString = Keys.ToArrayString();
//             KeysIndex = Keys.ToIndexArray();
//             Values = cache.Values.ToArray();
//             return this;
//         }
//
//         public TKey GetKeyByIndex(int index)
//         {
//             return Keys[index];
//         }
//
//         public TValue GetValueByIndex(int index)
//         {
//             return Values[index];
//         }
//     }
// }