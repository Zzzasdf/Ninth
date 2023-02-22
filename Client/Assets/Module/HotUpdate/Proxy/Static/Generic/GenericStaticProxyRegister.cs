using System;

namespace Ninth.HotUpdate
{
    // 泛型静态代理登记表
    public static class GenericStaticProxyRegister
    {
        private static Action m_RegisterClearFunc;

        public static void RegisterClearFunc(Action clearFun)
        {
            m_RegisterClearFunc += clearFun;
        }

        public static void ClearAll()
        {
            m_RegisterClearFunc?.Invoke();
        }
    }
}