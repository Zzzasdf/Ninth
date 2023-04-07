using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.HotUpdate
{
    public class ActionHandler
    {
        public event Action OnRegister;
        public void OnTrigger() => OnRegister?.Invoke();
    }
}