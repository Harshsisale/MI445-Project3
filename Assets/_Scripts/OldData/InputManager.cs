using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 movement;
    public static Vector2 cursor;
    public static bool usingGamepad;

    public static bool boostHeld;
    public static bool dashPressed;
    public static bool interactPressed;

    private InputAction moveAction;
    private InputAction boostAction;
    private InputAction dashAction;
    private InputAction interactAction;
    private InputAction cursorPos;
    private InputAction aimAction;

    private static Vector2 stickAim;
    private static Vector2 lastAimDirection = Vector2.right;
    private const float AimDeadzone = 0.2f;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        boostAction = playerInput.actions["Boost"];
        dashAction = playerInput.actions["Dash"];

        interactAction = playerInput.actions["Interact"];
        
        cursorPos = playerInput.actions["Cursor"];
        aimAction = playerInput.actions["Aim"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        cursor = cursorPos.ReadValue<Vector2>();
        stickAim = aimAction.ReadValue<Vector2>();

        boostHeld = boostAction.IsPressed();
        
        dashPressed = dashAction.WasPressedThisFrame();
        interactPressed = interactAction.WasPressedThisFrame();

        if (stickAim.sqrMagnitude > AimDeadzone * AimDeadzone) 
            usingGamepad = true;
        else if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.1) 
            usingGamepad = false;

        HandleVibration();
    }

    public static Vector2 GetAimDirection(Vector3 origin)
    {
        if (usingGamepad)
        {
            if (stickAim.sqrMagnitude > AimDeadzone * AimDeadzone)
            {
                lastAimDirection = stickAim.normalized;
            }
            return lastAimDirection;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(cursor);
        Vector2 dir = (Vector2)(mouseWorldPos - origin);

        if (dir.sqrMagnitude > 0.001f)
        {
            lastAimDirection = dir.normalized;
        }
        return lastAimDirection;
    }

    private void HandleVibration()
    {
        if (Gamepad.current == null) return;

        if (boostHeld) Gamepad.current.SetMotorSpeeds(0.05f, 0.1f);
        else Gamepad.current.ResetHaptics();
    }

    private void OnDisable()
    {
        if (Gamepad.current != null) Gamepad.current.ResetHaptics();
    }
}