using System;
using System.Reflection;
using UnityEngine;
using VContainer.Unity;

namespace Ninth
{
    public class LoadCSharp : IStartable
    {
        private string hotUpdateAssembly = "HotUpdateMain";
        private string hotUpdateDriverType = "Ninth.HotUpdate.GameDriver";
        private string hotUpdateDriverEntry = "Init";
        
        public void Start()
        {
            try
            {
                var assembly = Assembly.Load(hotUpdateAssembly);
                var type = assembly.GetType(hotUpdateDriverType);
                var mainMethod = type.GetMethod(hotUpdateDriverEntry);
                mainMethod.Invoke(null, null);
            }
            catch (Exception e)
            {
                e.Error();
            }
        }
    }
}
