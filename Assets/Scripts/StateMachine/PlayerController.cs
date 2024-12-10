using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerLocomotionActions
{
    [Header("Components")]
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    private Transform _playerModel;

    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float drag = 0.1f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public float jumpForce = 5f; // Fuerza del salto
    private bool isGrounded;
    private bool isCrouching = false;

    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerControls controls;

    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.PlayerLocomotion.Enable();
        controls.PlayerLocomotion.Look.performed += OnLook;
        controls.PlayerLocomotion.Jump.performed += OnJump; // Vincula el evento de salto
        controls.PlayerLocomotion.Crouch.started += OnCrouch; // Vincula el evento de inicio de agacharse
        controls.PlayerLocomotion.Crouch.canceled += OnCrouch; // Vincula el evento de finalizar agacharse
    }

    private void OnDisable()
    {
        controls.PlayerLocomotion.Look.performed -= OnLook;
        controls.PlayerLocomotion.Jump.performed -= OnJump;
        controls.PlayerLocomotion.Crouch.started -= OnCrouch;
        controls.PlayerLocomotion.Crouch.canceled -= OnCrouch;
        controls.PlayerLocomotion.Disable();
    }

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerModel = transform.GetChild(0); 
        
        _playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        CheckGround();

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

        if (isGrounded)
        {
            Vector3 movementDelta = movementDirection * (runAcceleration * Time.deltaTime);
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;

            _characterController.Move(newVelocity * Time.deltaTime);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        
        Ray ray = _playerCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
        {
            Vector3 lookDirection = hitInfo.point - transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                _playerModel.rotation = Quaternion.RotateTowards(_playerModel.rotation, targetRotation, 500 * Time.deltaTime);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) // Verifica si el salto se realizó y el personaje está en el suelo
        {
            _playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started) // Cuando el jugador empieza a agacharse
        {
            isCrouching = true;
            runSpeed /= 2; // Reducir velocidad al agacharse
        }
        else if (context.canceled) // Cuando el jugador deja de agacharse
        {
            isCrouching = false;
            runSpeed *= 2; // Restaurar velocidad al dejar de agacharse
        }
    }
}
