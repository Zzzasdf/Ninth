using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniTaskTutorial.BaseUsing.Scripts
{
    public class Runner : MonoBehaviour
    {
        public float Speed;
        public Transform Target;
        public Vector3 StartPos { get; private set; }
        public bool ReachGoal { get; set; }

        private void Awake()
        {
            StartPos = this.transform.position;
        }

        public void Reset()
        {
            ReachGoal = false;
            this.transform.position = StartPos;
        }
    }
}