using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour, PlayerInput.IPlayerControlsActions
{
    [SerializeField] private bool canControl;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float deadzone = 0.1f;

    public float moveSpeed;
    private Vector2 moveAxis;

    private PlayerInput actions;
    private InputAction move;

    private void Awake()
    {
        actions = new PlayerInput();
        actions.PlayerControls.SetCallbacks(this);
    }

    public void EnableControls()
    {
        ToggleCanControl(true);
        actions.PlayerControls.Enable();
    }

    public void DisableControls()
    {
        ToggleCanControl(false);
        actions.PlayerControls.Disable();
    }


    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        /*if (!canControl)
        {
            ResetMoveAxis();
            return;
        }
        
        moveAxis = context.ReadValue<Vector2>();

        if (moveAxis.magnitude < deadzone)
            ResetMoveAxis();*/
    }

    private void ResetMoveAxis()
    {
        moveAxis = new Vector2(0, 0);
    }

    private void Update()
    {
        player.Move(moveAxis);

        if (!canControl)
        {
            ResetMoveAxis();
            return;
        }

        if (Input.GetKey(KeyCode.W))
            moveAxis.x = -1 * moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            moveAxis.x = 1 * moveSpeed * Time.deltaTime;
        else
            moveAxis.x = 0;

        if (Input.GetKey(KeyCode.A))
            moveAxis.y = -1 * moveSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            moveAxis.y = 1 * moveSpeed * Time.deltaTime;
        else
            moveAxis.y = 0;
    }

    public void ToggleCanControl(bool b)
    {
        canControl = b;
    }
}