using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ODMPlayerController : MonoBehaviour
{
    public ODMStats odmStats;
    public ODMStats slowedOdmStats;
    private ODMStats currentStats;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public static Transform Instance;

    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private Transform gravSource;
    [SerializeField] private LayerMask overlapLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStats = odmStats;

        Instance = this.transform;
    }

    void Update()
    {
        ProcessInputs();
        
    }

    void FixedUpdate()
    {
        if (IsOverlapping()) { currentStats = slowedOdmStats; }
        else { currentStats = odmStats; }

        FaceCursor();
        Move();

        ApplyPullForce();
    }

    private void ProcessInputs()
    {
        float moveX = InputManager.movement.x;
        float moveY = InputManager.movement.y;

        moveDirection = new Vector2(moveX, moveY).normalized;

        //Debug.Log($"Move X: {moveX} | Move Y: {moveY}");
    }

    private void Move()
    {
        float maxSpeed = InputManager.boostHeld ? currentStats.MaxRunSpeed : currentStats.MaxWalkSpeed;
        Vector2 targetVelocity = moveDirection * maxSpeed;
        
        float rate = moveDirection.sqrMagnitude > 0.01f ? currentStats.Acceleration : currentStats.Drag;

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, rate * Time.fixedDeltaTime);

        //Alternative movement mechanic using rigidbody forces. More acccurate to physics, less controllable
        /*
        rb.AddForce(moveDirection * currentStats.Acceleration, ForceMode2D.Force);

        if (rb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        */
    }

    private void FaceCursor()
    {
        Vector2 aim = InputManager.GetAimDirection(transform.position);

        // -90 because the sprite points up
        float targetAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg - 90f;

        float newAngle = turnSpeed > 0f ? Mathf.MoveTowardsAngle(rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime) : targetAngle;

        rb.MoveRotation(newAngle);
    }

    private void ApplyPullForce()
    {
        if (gravSource == null) { return; }

        // Pull direction
        Vector2 toGravSource = (gravSource.position - transform.position);
        float distance = toGravSource.magnitude;
        float forceStrength = currentStats.PullStrength * Mathf.Pow(distance, currentStats.GrowthExponent);

        if (distance < currentStats.MinPullDistance) { return; } // close enough; no pull

        Vector2 pullDir = toGravSource.normalized;
        Vector2 pullForce = pullDir * forceStrength;

        rb.AddForce(pullForce, ForceMode2D.Force);
    }

    private bool IsOverlapping()
    {
        float radius = 0.3f;
        Collider2D overlapCheck = Physics2D.OverlapCircle(transform.position, radius, overlapLayer);

        if (overlapCheck != null)
        {
            return true;
        }
        return false;
    }
}