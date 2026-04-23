using UnityEngine;
using UnityEngine.Scripting;

namespace MetalheadGame
{
    public class LocomotionController : MonoBehaviour
    {
        public bool IsGrounded => _characterController.isGrounded;
        public float VerticalVelocity { get; private set; }
        
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;

        [Header("Movement")]
        [SerializeField] private float _walkSpeed = 2.4f;
        [SerializeField] private float _runSpeed = 6.5f;
        [SerializeField] private float _backwardMultiplier = 0.75f;
        [SerializeField] private float _rotationSpeed = 300f;

        [Header("Gravity & Jump")]
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _jumpHeight = 0.4f;

        private Vector2 _moveInput;
        private bool _isRunning;

        public void SetMoveInput(Vector2 moveInput) => _moveInput = moveInput;
        public void SetRunning(bool isRunning) => _isRunning = isRunning;

        private void Update()
        {
            HandleGravity();
            HandleRotation();
            HandleMovement();
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded && VerticalVelocity < 0f)
            {
                VerticalVelocity = -2f;
            }

            VerticalVelocity += _gravity * Time.deltaTime;
        }

        private void HandleRotation()
        {
            transform.Rotate(0f, _moveInput.x * _rotationSpeed * Time.deltaTime, 0f);
        }

        private void HandleMovement()
        {
            var forwardInput = _moveInput.y;
            var moveSpeed = _isRunning ? _runSpeed : _walkSpeed;
            if (forwardInput < 0f)
            {
                moveSpeed *= _backwardMultiplier;
            }

            var movement = transform.forward * (forwardInput * moveSpeed);
            var velocity = movement + Vector3.up * VerticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);
        }

        [Preserve] // Called by the Animator
        public void ApplyJumpImpulse()
        {
            VerticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}
