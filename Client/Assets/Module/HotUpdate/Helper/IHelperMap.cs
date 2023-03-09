using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public abstract class IHelperMap<TEnum, TSelf> : Singleton<TSelf>
        where TEnum: Enum
        where TSelf : new()
    {
        private Dictionary<TEnum, Type> m_Mode2Type;
        private Dictionary<Type, TEnum> m_Type2Mode;

        public IHelperMap()
        {
            m_Mode2Type = new Dictionary<TEnum, Type>();
            m_Type2Mode = new Dictionary<Type, TEnum>();
            Assembler();
        }

        protected abstract void Assembler();

        protected void Assembler(params (TEnum mode, Type type)[] tuples)
        {
            for (int index = 0; index < tuples.Length; index++)
            {
                m_Mode2Type.Add(tuples[index].mode, tuples[index].type);
                m_Type2Mode.Add(tuples[index].type, tuples[index].mode);
            }
        }

        public Type GetType(TEnum mode)
        {
            return m_Mode2Type[mode];
        }

        public TEnum GetMode<T>()
        {
            return m_Type2Mode[typeof(T)];
        }
    }
}