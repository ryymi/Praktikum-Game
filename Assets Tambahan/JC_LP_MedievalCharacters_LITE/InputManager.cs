using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Project_Praktikum.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;
        
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }
        public bool Jump { get; private set; } //  Tambahkan properti Jump

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _runAction;
        private InputAction _jumpAction;

        private void Awake()
        {
            _currentMap = PlayerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump"); //  Ambil action Jump

            _moveAction.performed += onMove;
            _moveAction.canceled += onMove;

            _lookAction.performed += onLook;
            _lookAction.canceled += onLook;

            _runAction.performed += onRun;
            _runAction.canceled += onRun;

            _jumpAction.performed += onJump;  //  Tambah event listener
            _jumpAction.canceled += onJump;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void onJump(InputAction.CallbackContext context) //  Fungsi untuk Jump
        {
            Jump = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }
    }
}
