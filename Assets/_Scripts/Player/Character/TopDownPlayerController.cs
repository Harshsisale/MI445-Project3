using System.Diagnostics;
using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private bool flipX;
    
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        float moveX = InputManager.movement.x;
        float moveY = InputManager.movement.y;

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveX > 0.1f) flipX = true;
        else if (moveX < -0.1f) flipX = false;
    }

    private void Move()
    {
        rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, flipX ? 180f : 0f);

        // animation stuff? State machine, maybe?
        
    }
}
