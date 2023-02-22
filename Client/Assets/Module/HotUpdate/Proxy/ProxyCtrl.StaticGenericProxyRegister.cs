using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class ProxyCtrl
    {
        private Action m_StaticGenericProxyRegisterClear;

        public partial void StaticGenericProxyRegisterClear(Action clear)
        {
            m_StaticGenericProxyRegisterClear = clear;
        }

        public partial void StaticGenericProxyClearAll()
        {
            m_StaticGenericProxyRegisterClear?.Invoke();
        }
    }
}