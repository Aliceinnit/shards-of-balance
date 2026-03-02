using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 3f;

    [Header("Jump")]
    public float jumpForce = 500f;              // force value (we multiply by 10f like your old code)
    public bool doubleJumpUnlocked = false;     // tick TRUE in Level 2 (or set from a manager)
    private int jumpsUsed = 0;

    [Header("Gravity")]
    public float groundedGravity = 2.1f;
    public float airGravity = 2.1f;
    public float fastFallGravity = 2.1f;

    [Header("Collectibles")]
    public StarManager sm;

    [Header("Visuals")]
    public Transform rigRoot;                   // assign your FLIP wrapper here (NOT the Player collider object)
    public float animatorSpeed = 1f;

    private Rigidbody2D rb;
    private Animator animator;

    private float horizontalInput;
    private bool jumpRequested;
    private bool fastFallRequested;

    private bool isGrounded = true;
    private int facing = 1; // 1 = right, -1 = left

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = groundedGravity;

        animator = GetComponentInChildren<Animator>();
        if (animator) animator.speed = animatorSpeed;
    }

    private void Update()
    {
        // input
        horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1f;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;

        fastFallRequested = Input.GetKey(KeyCode.S);

        // animator params
        if (animator)
        {
            animator.SetFloat("speed", Mathf.Abs(horizontalInput));
            animator.SetBool("isGrounded", isGrounded);
        }

        // Footsteps while walking on ground
        if (AudioManager.Instance != null)
        {
            if (Mathf.Abs(horizontalInput) > 0.01f && isGrounded)
                AudioManager.Instance.StartFootsteps();
            else
                AudioManager.Instance.StopFootsteps();
        }
    }

    private void FixedUpdate()
    {
        // horizontal move
        var vel = rb.linearVelocity;
        vel.x = horizontalInput * movementSpeed;
        rb.linearVelocity = vel;

        // gravity
        rb.gravityScale = isGrounded ? groundedGravity : (fastFallRequested ? fastFallGravity : airGravity);

        // jump / double jump
        int maxJumps = doubleJumpUnlocked ? 2 : 1;

        if (jumpRequested && jumpsUsed < maxJumps)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);

            // make jump consistent
            vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;

            rb.AddForce(Vector2.up * jumpForce * 10f);

            jumpsUsed++;
            isGrounded = false;
        }

        jumpRequested = false;
    }

    private void LateUpdate()
{
    if (!rigRoot) return;

    if (horizontalInput > 0.01f)
        facing = 1;
    else if (horizontalInput < -0.01f)
        facing = -1;

    Vector3 s = rigRoot.localScale;

    // Flip on Z
    s.z = Mathf.Abs(s.z) * facing;

    rigRoot.localScale = s;
}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpsUsed = 0; // reset jumps when landing
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("star"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.wallTouch);

            Destroy(other.gameObject);
            if (sm != null) sm.StarCount++;
        }
    }

    // Call this from LevelManager when Level 2 starts (optional)
    public void UnlockDoubleJump()
    {
        doubleJumpUnlocked = true;
    }
}