using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionActions
{
    private PlayerControls.IPlayerLocomotionActions _playerLocomotionActionsImplementation;
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
        
        PlayerControls.PlayerLocomotion.Enable();
        PlayerControls.PlayerLocomotion.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerLocomotion.Disable();
        PlayerControls.PlayerLocomotion.RemoveCallbacks(this);
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }
}