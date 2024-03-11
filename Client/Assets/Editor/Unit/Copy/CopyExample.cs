using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using NUnit.Framework;
using UnityEngine;

namespace Ninth.Editor
{
    public class CopyExample 
    {
        public class Foo
        {
            public int A;
            private int B;
            public int C { get; set; }
            public int D { get; }
            private int E { get; set; }
            public List<int> F = new List<int>();
            private List<string> G = new List<string>();

            public Foo()
            {
                
            }
            public Foo(int B, int D, int E, List<string> G)
            {
                this.B = B;
                this.D = D;
                this.E = E;
                this.G = G;
            }
        }
        
        [Test]
        public static void ShallowCopy()
        {
            var foo = new Foo(2, 4,5, new List<string> { "G", "G" })
            {
                A = 1,
                C = 3,
                F = new List<int> { 6, 6 },
            };
            var cache = new Dictionary<int, Foo>();
            cache.Add(1, new Foo());
            var foo1 = cache[1];
            cache[1] = CopyHelper.ShallowCopy(foo);
            foo.F[0] = 66;
            foo1.F.Log();
        }
    }
}