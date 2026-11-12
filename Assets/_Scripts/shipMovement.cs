using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class shipMovement : MonoBehaviour
{

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 10.0f;

    [Tooltip("How slippery movement is in percentage.")]
    public float friction = 30.0f;
    private Vector3 velocity;

    [Header("Input Actions & Controls")]
    [Tooltip("The input action(s) that map to player movement")]
    public InputAction moveAction;

    void OnEnable()
    {
        moveAction.Enable();
    }

        void OnDisable()
    {
        moveAction.Disable();
    }

    private void Start()
    {
        if (moveAction.bindings.Count == 0)
        {
            Debug.LogWarning("Input binding not set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
     handleInput();   
    }

    private void handleInput()
    {
        // Get movement input from the inputManager
        if (moveAction.bindings.Count == 0)
        {
            Debug.LogError("Move binding not set.");
        }
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontalMovement = moveInput.x;
        float verticalMovement = moveInput.y;
        Vector3 movementVector = new Vector3(horizontalMovement, verticalMovement, 0);
        // Move the player
        MovePlayer(movementVector);
    }

    private void MovePlayer(Vector3 movement)
    {
        velocity += movement * moveSpeed * Time.deltaTime;
        velocity *= Mathf.Pow(1f - friction / 100f, Time.deltaTime);
        transform.position = transform.position + (velocity * Time.deltaTime);
    }


}
