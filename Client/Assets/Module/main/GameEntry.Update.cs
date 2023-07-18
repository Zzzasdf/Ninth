using System;
using UnityEngine;

namespace Ninth
{
    public partial class GameEntry
    {
        private static Action<float> m_RegisterUpdate = null;

        public static void RegisterUpdate(Action<float> func)
        {
            m_RegisterUpdate += func;
        }

        public static void CancelUpdate(Action<float> func)
        {
            m_RegisterUpdate -= func;
        }

        void Update()
        {
            m_RegisterUpdate?.Invoke(Time.deltaTime);
            Test();
        }
    }
}