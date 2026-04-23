using System;
using UnityEngine;

namespace MetalheadGame
{
    public class AnimationController : MonoBehaviour
    {
        [Header("Animations")] 
        [SerializeField] private string _speedFloatName = "Speed";
        [SerializeField] private string _isGroundedBoolName = "Is Grounded";
        [SerializeField] private string _jumpTriggerName = "Jump";
        [SerializeField] private string _rotationFloatName = "Rotation";

        [Header("Setup")] 
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedSmoothTime = 0.15f;

        private Vector2 _moveInput;
        private bool _isRunning;
        private bool _isGrounded;
        private float _currentSpeed;
        private float _speedVelocity;

        private int _speedParam;
        private int _isGroundedParam;
        private int _jumpParam;
        private int _rotationParam;

        private void Awake()
        {
            _speedParam = Animator.StringToHash(_speedFloatName);
            _isGroundedParam = Animator.StringToHash(_isGroundedBoolName);
            _jumpParam = Animator.StringToHash(_jumpTriggerName);
            _rotationParam = Animator.StringToHash(_rotationFloatName);
        }

        private void Update()
        {
            var forwardInput = _moveInput.y;
            var targetSpeed = forwardInput * (_isRunning ? 1f : 0.5f);
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedVelocity, _speedSmoothTime);

            _animator.SetFloat(_speedParam, _currentSpeed);
            _animator.SetFloat(_rotationParam, _moveInput.x);
            _animator.SetBool(_isGroundedParam, _isGrounded);
        }

        public void SetMoveInput(Vector2 moveInput) => _moveInput = moveInput;
        public void SetRunning(bool isRunning) => _isRunning = isRunning;
        public void SetGrounded(bool isGrounded) => _isGrounded = isGrounded;
        public void PlayJump() => _animator.SetTrigger(_jumpParam);
    }
}