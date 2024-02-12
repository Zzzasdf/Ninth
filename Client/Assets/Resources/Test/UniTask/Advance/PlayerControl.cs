using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniTaskTutorial.Advance.Scripts
{
    [Serializable]
    public struct ControlParams
    {
        [Header("旋转速度")] public float rotateSpeed;
        [Header("移动速度")] public float moveSpeed;
        [Header("开枪最小间距")] public float fireInterval;
    }
    
    public class PlayerControl
    {
        private Transform _playerRoot;
        private ControlParams _controlParams;

        public UnityEvent OnFire;

        private float _lastFireTime;

        public PlayerControl(Transform playerRoot, ControlParams controlParams)
        {
            _playerRoot = playerRoot;
            _controlParams = controlParams;
        }
        
        // 启动输入检测
        private void StartCheckInput()
        {
            CheckPlayerInput().ForEachAsync((delta) =>
            {
                _playerRoot.position += delta.Item1;
                _playerRoot.forward = Quaternion.AngleAxis(delta.Item2, Vector3.up) * _playerRoot.forward;
                if (delta.Item3 - _lastFireTime > _controlParams.fireInterval)
                {
                    OnFire?.Invoke();
                    _lastFireTime = delta.Item3;
                }
            }, _playerRoot.GetCancellationTokenOnDestroy()).Forget();
        }

        private IUniTaskAsyncEnumerable<(Vector3, float, float)> CheckPlayerInput()
        {
            return UniTaskAsyncEnumerable.Create<(Vector3, float, float)>(async (writer, token) =>
            {
                await UniTask.Yield();
                while (!token.IsCancellationRequested)
                {
                    await writer.YieldAsync((GetInputMoveValue(), GetInputAxisValue(), GetIfFired()));
                    await UniTask.Yield();
                }
            });
        }

        private float GetIfFired()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return Time.time;
            }

            return -1;
        }

        private Vector3 GetInputMoveValue()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            Vector3 move = (_playerRoot.forward * vertical + _playerRoot.right * horizontal) *
                           (_controlParams.moveSpeed * Time.deltaTime);
            return move;
        }

        private float GetInputAxisValue()
        {
            if (!Input.GetMouseButton(1)) return default;
            var result = Input.GetAxis("Mouse X") * _controlParams.rotateSpeed;
            return Mathf.Clamp(result, -90, 90);
        }

        public void Start()
        {
            StartCheckInput();
        }
    }
}
