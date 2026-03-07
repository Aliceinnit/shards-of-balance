using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 3f;

    [Header("Jump")]
    public float jumpForce = 500f;
    private int jumpsUsed = 0;
    private const int maxJumps = 2;

    [Header("Knockback")]
    public float knockbackForce;
    public float knockbackUpForce;

    [Header("Gravity")]
    public float groundedGravity = 2.1f;
    public float airGravity = 2.1f;
    public float fastFallGravity = 2.1f;

    [Header("References")]
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
    private bool isDead = false;


    private const string PortalInTag = "PortalIn";

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dasingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    [SerializeField] private bool dashUnlocked = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = groundedGravity;

        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.speed = animatorSpeed;
    }

    private void Update()
    {
        if (isDead) return;
        if (isDashing) return;

        // input
        horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1f;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;

        fastFallRequested = Input.GetKey(KeyCode.S);

        // animator params
        if (animator != null)
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

        //Dash
        if (dashUnlocked && Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (isDashing) return;

        // horizontal move (Unity 6 uses linearVelocity)
        Vector2 vel = rb.linearVelocity;
        vel.x = horizontalInput * movementSpeed;
        rb.linearVelocity = vel;

        // gravity
        rb.gravityScale = isGrounded ? groundedGravity : (fastFallRequested ? fastFallGravity : airGravity);

        // jump
        if (jumpRequested && jumpsUsed < maxJumps)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayJump();

            if (animator != null)
            {
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
                animator.Play("Armature|Ase-jump", 0, 0f);
            }

            // consistent jump
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
        if (isDead) return;
        if (rigRoot == null) return;

        if (horizontalInput > 0.01f) facing = 1;
        else if (horizontalInput < -0.01f) facing = -1;

        Vector3 s = rigRoot.localScale;

        // Recommended 2D flip: X axis
        s.x = Mathf.Abs(s.x) * facing;

        rigRoot.localScale = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpsUsed = 0;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("star"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayPickup();

            if (sm != null)
                sm.AddStar();
            else
                Debug.LogWarning("Player.sm is not assigned.");

            Destroy(other.gameObject);
        }

        if (AudioManager.Instance != null && TagExists(PortalInTag) && other.CompareTag(PortalInTag))
        {
            AudioManager.Instance.PlayPortalIn();
        }
    }

    // Call this from hazards/killzones/enemies
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopFootsteps();
            AudioManager.Instance.PlayDeath();
        }

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Invoke(nameof(Respawn), 0.4f); // wait so the sound plays
    }

    void Respawn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ApplyKnockback()
    {
        float direction = Mathf.Sign(rb.linearVelocity.x);
 
        if (direction == 0)
        {
            direction = -1;
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * knockbackForce, knockbackUpForce), ForceMode2D.Impulse);
    }

    // Prevents Console spam if the tag is missing.
    private static bool TagExists(string tag)
    {
        // Unity throws if tag doesn't exist — catch it and return false.
        try
        {
            // This will throw if the tag isn't defined.
            GameObject.FindWithTag(tag);
            return true;
        }
        catch
        {
            return false;
        }
    }

    //Dash
    private IEnumerator Dash()
{
    canDash = false;
    isDashing = true;

    if (animator != null) animator.SetBool("isDashing", true);

    float originalGravity = rb.gravityScale;
    rb.gravityScale = 0f;

    rb.linearVelocity = new Vector2(facing * dashingPower, 0f);

    if (tr != null) tr.emitting = true;

    yield return new WaitForSeconds(dashingTime);

    if (tr != null) tr.emitting = false;

    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

    rb.gravityScale = originalGravity;
    isDashing = false;

    if (animator != null) animator.SetBool("isDashing", false);

    yield return new WaitForSeconds(dasingCooldown);
    canDash = true;
}

}