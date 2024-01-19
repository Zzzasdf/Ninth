using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ninth.Editor
{
    public class TaskExample
    {
        public void Foo()
        {
            IOCExample ioc = new IOCExample();
            string? a = ioc.GetStr(1);
        }
    }
}
