using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdate
{
    [DisallowMultipleComponent]
    public sealed class GameDriver : MonoBehaviour
    {
        private GameDriver() { }

        private void Awake()
        {
            "热更部分启动成功！！".Log();
            DontDestroyOnLoad(this);
            GameEntryConfig.Port.Log();
        }
    }
}
