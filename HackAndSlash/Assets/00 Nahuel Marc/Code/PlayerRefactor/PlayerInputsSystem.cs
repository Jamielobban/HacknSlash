using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsSystem : MonoBehaviour
{

    private PlayerManager _player;
    private PlayerInputActionsRefactor _action;

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
        _action = new PlayerInputActionsRefactor();

        _action.Player.Enable();

        // -- Movement inputs -- //
        _action.Player.Movement.performed += MoveRightStick_performed;
        _action.Player.Movement.canceled += MoveRightStick_canceled;
        _action.Player.R2.performed += R2_performed;        
        _action.Player.R2.canceled += R2_canceled;        
        _action.Player.Jump.performed += Jump_performed;
        _action.Player.Dash.performed += Dash_performed;


        // -- Attack inputs -- // 
        _action.Player.L2.performed += L2_performed;
        _action.Player.Square.performed += Square_performed;
        _action.Player.Triangle.performed += Triangle_performed;

        // -- Camera and Settings inputs -- //
        _action.Player.Rotation.performed += MoveLeftStick_performed;
        _action.Player.Rotation.canceled += MoveLeftStick_canceled;
        _action.Player.Select.performed += Select_performed;
        _action.Player.Interact.performed += Interact_performed;
    }

    public Vector2 GetDirectionLeftStick() => _action.Player.Movement.ReadValue<Vector2>();
    public Vector2 GetDirectionRightStick() => _action.Player.Rotation.ReadValue<Vector2>();

    private void MoveRightStick_performed(InputAction.CallbackContext context)
    {

    }
    private void MoveRightStick_canceled(InputAction.CallbackContext context)
    {

    }
    private void MoveLeftStick_performed(InputAction.CallbackContext context)
    {

    }
    private void MoveLeftStick_canceled(InputAction.CallbackContext context)
    {

    }

    private void R2_performed(InputAction.CallbackContext context)
    {
    }
    private void R2_canceled(InputAction.CallbackContext context)
    {
    }
    private void L2_performed(InputAction.CallbackContext context)
    {
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
    }
    private void Dash_performed(InputAction.CallbackContext context)
    {
    }
    private void Square_performed(InputAction.CallbackContext context)
    {
    }
    private void Triangle_performed(InputAction.CallbackContext context)
    {
    }
    private void Select_performed(InputAction.CallbackContext context)
    {
    }
    private void Interact_performed(InputAction.CallbackContext context)
    {
    }

}
