using System;
using System.Reflection;
using UnityEngine;

namespace Ninth.Utility
{
    public static class CopyHelper
    {
        public static T ShallowCopy<T>(T source)
        {   
            var target = (T)Activator.CreateInstance(typeof(T));
            ShallowCopy(target, source);
            return target;
        }
        
        public static void ShallowCopy<T>(T target, T source)
        {
            if (target == null || source == null) return;
            var type = source.GetType();  
            // 遍历源对象的所有字段  
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))  
            {  
                field.SetValue(target, field.GetValue(source));  
            }  
            // 遍历源对象的所有属性（假设属性有setter）  
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))  
            {  
                if (property.CanWrite)  
                {  
                    property.SetValue(target, property.GetValue(source, null), null);  
                }  
            }  
        }

        public static T DeepCopy<T>(T source)
        {
            var json = JsonUtility.ToJson(source);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
