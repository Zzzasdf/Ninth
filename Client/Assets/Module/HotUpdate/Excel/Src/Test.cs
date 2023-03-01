using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class TestRow
    {
        /// <summary>
        /// 
        /// </summary>
        public int K { get; private set; }

        public int A { get; private set; }

        public string C { get; private set; }

        public int2 D { get; private set; }

        public float3 E { get; private set; }

        public float4 F { get; private set; }

        public List<int> G { get; private set; }

        public string[] H { get; private set; }

        public List<float2> I { get; private set; }

        public TestRow()
        {
            D = new int2();
            E = new float3();
            F = new float4();
            G = new List<int>();
            H = new string[4];
            I = new List<float2>();
        }
    }

    public class Test
    {
        private Dictionary<int, TestRow> m_Cache;

        private Test()
        {
            m_Cache = new Dictionary<int, TestRow>();
            // 解码
        }

        public TestRow this[int key] 
        {
            get
            {
                return m_Cache[key];
            }
        }
    }
}