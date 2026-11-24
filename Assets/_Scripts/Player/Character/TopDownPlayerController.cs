using System.Diagnostics;
using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    public enum Facing{Side, Up, Down}

    [SerializeField] private Facing facingDirection;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private bool flipX;
    [SerializeField] private SpriteRenderer playerSprite;
    
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool isMoving;

    public Facing FacingDirection => facingDirection;
    public bool FlipX => flipX;
    public bool IsMoving => isMoving;

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
        isMoving = moveDirection.sqrMagnitude > 0.01f;

        SetFacing();

        if (moveX > 0.1f) flipX = true;
        else if (moveX < -0.1f) flipX = false;
    }

    private void SetFacing()
    {
        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            facingDirection = Facing.Side;
        }
        else if (moveDirection.y > 0.1)
        {
            facingDirection = Facing.Up;
        }
        else
        {
            facingDirection = Facing.Down;
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime);

        playerSprite.flipX = !flipX;
        // animation stuff? State machine, maybe?
        
    }
}
