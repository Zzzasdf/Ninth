using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.HotUpdate;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using System.Linq;

// TODO => ioc: zenject
// TODO => 可空的引用类型 C#8.0有但默认不开启，C#10默认开启。开启方式 #nullable enable
namespace Ninth.Editor
{
    public class IOCExample
    {
        [Test]
        public void Foo()
        {
            // Dictionary<int, int> temp = new();
            // int result = 1;
            // temp.TryGetValue(1, out result);
            // Debug.Log(result);
            // Assert.Equals(result, 1);
            // int a = 1;
            // int c = 2;
            // int b = a switch
            // {
            //     > 0 and < 10 => 1,
            //     < 0 or < 10 => -1,
            //     > 0 when c > 10 => 2,
            //     _ => 0
            // };
        }
    }
}