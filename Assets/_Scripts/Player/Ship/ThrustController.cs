using UnityEngine;

public class ThrustController : MonoBehaviour
{
    public ThrustStats thrustStats;
    public ThrustStats slowedThrustStats;
    private ThrustStats currentStats;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public static Transform Instance;

    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private Transform gravSource;
    [SerializeField] private LayerMask overlapLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStats = thrustStats;

        Instance = this.transform;
    }

    void FixedUpdate()
    {
        currentStats = IsOverlapping() ? slowedThrustStats : thrustStats;

        FaceCursor();
        Move();

        ApplyPullForce();
    }

    private void Move()
    {
        float maxSpeed = InputManager.boostHeld ? currentStats.MaxRunSpeed : currentStats.MaxWalkSpeed;
        InputManager.HandleVibration();

        if (InputManager.propelHeld)
        {
            Vector2 forward = Quaternion.Euler(0f, 0f, rb.rotation) * Vector2.up;
            Vector2 velocity = rb.linearVelocity + forward * currentStats.Acceleration * Time.fixedDeltaTime;
            
            rb.linearVelocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, currentStats.Drag * Time.fixedDeltaTime);
        }
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