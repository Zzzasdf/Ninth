using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// TODO => ioc: zenject
// TODO => 可空的引用类型 C#8.0有但默认不开启，C#10默认开启。开启方式 #nullable enable
#nullable enable

namespace UnityEditor
{
    public class IOCExample
    {
        public Dictionary<int, string> dic = new();

        public string? GetStr(int id)
        {
            dic.TryGetValue(id, out string result);
            return result;
        }

        public void Main()
        {
           string a = GetStr(1);
        }
    }
}