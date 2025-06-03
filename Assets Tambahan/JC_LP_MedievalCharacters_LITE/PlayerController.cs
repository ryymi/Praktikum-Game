using System;
using UnityEngine;
using Project_Praktikum.Manager;

namespace  Project_Praktikum.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float JumpForce = 5f; //  Tambahkan ini

        private Rigidbody _playerRigidBody;
        private InputManager _inputManager;

        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private float _xRotation;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;

        private Vector2 _currentVelocity;
        private bool _isGrounded = true; //  Tambahkan untuk mengecek apakah menyentuh tanah

        void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidBody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void FixedUpdate()
        {
            Move();
            HandleJump(); //  Tambahkan ini
        }

        private void LateUpdate()
        {
            CamMovement();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0;

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _playerRigidBody.linearVelocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidBody.linearVelocity.z;

            _playerRigidBody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void HandleJump() //  Fungsi lompat
        {
            if (_inputManager.Jump && _isGrounded)
            {
                _playerRigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                _isGrounded = false;
            }
        }

        private void CamMovement()
        {
            if (!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;

            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerRigidBody.MoveRotation(_playerRigidBody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
        }
    }
}
