using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniTaskTutorial.Advance.Scripts
{
    public class PlayerControlSample : MonoBehaviour
    {
        [Header("玩家")] 
        [SerializeField] 
        private Transform playerRoot;
        
        [Header("控制参数")] 
        [SerializeField] 
        private ControlParams controlParams;
        
        [SerializeField] 
        private UnityEvent onFireEvent;

        private void Start()
        {
            PlayerControl playerControl = new PlayerControl(playerRoot, controlParams);
            playerControl.OnFire = onFireEvent;
            playerControl.Start();
        }
    }
}