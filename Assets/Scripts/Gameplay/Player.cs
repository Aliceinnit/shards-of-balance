using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 3f;

    public float jumpForce = 500f;             
    private int jumpsUsed = 0;
    private const int maxJumps = 2;
    public float knockbackForce = 10f;

    public float groundedGravity = 2.1f;
    public float airGravity = 2.1f;
    public float fastFallGravity = 2.1f;

    public StarManager sm;

    public Transform rigRoot;                   
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

        if (jumpRequested && jumpsUsed < maxJumps)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jump);

            if (animator)
            {
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");

                //restart jump animation even if already playing
                animator.Play("Armature|Ase-jump", 0, 0f);
            }
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

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;

        rb.linearVelocity = Vector2.zero; 
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}