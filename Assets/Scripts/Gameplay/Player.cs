using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float jumpSpeed = 500f;
    private bool isGrounded = true;
    private Rigidbody2D rb;

   
    private float horizontalInput;
    private bool jumpRequested;
    private bool fastFallRequested;

    private const float groundedGravity = 2.1f;
    private const float airGravity = 2.1f;
    private const float fastFallGravity = 2.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = groundedGravity;
    }

    private void Update()
    {
        horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1f;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        fastFallRequested = Input.GetKey(KeyCode.S);
    }

    private void FixedUpdate()
    {
        var vel = rb.linearVelocity;
        vel.x = horizontalInput * movementSpeed;
        rb.linearVelocity = vel;

        if (isGrounded)
        {
            rb.gravityScale = groundedGravity;
        }
        else
        {
            rb.gravityScale = fastFallRequested ? fastFallGravity : airGravity;
        }

        if (jumpRequested && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpSpeed * 10);
            isGrounded = false;
        }

        jumpRequested = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
