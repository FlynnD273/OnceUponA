using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    public float XSpeed;
    public float JumpHeight;
    public float VarJumpTime;
    public float VarJumpGravScale;
    public float CoyoteTime;
    public float Grounding;

    public PlayerState State;

    private Stopwatch coyote;
    private Stopwatch startJump;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumping;
    private bool jumpReleased;

    private TimeSpan varJumpTimeSpan;
    private TimeSpan coyoteTimeSpan;

    private float px;
    private float gravScale;

    public enum PlayerState { Idle, Walking, Jumping, Falling }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        gravScale = rb.gravityScale;
        coyote = new Stopwatch();
        startJump = new Stopwatch();
        varJumpTimeSpan = new TimeSpan(0, 0, 0, 0, (int)(1000 * VarJumpTime));
        coyoteTimeSpan = new TimeSpan(0, 0, 0, 0, (int)(1000 * CoyoteTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (Math.Abs(rb.velocity.x) <= float.Epsilon)
            {
                State = PlayerState.Idle;
            }
            else
            {
                State = PlayerState.Walking;
            }
        }
        else
        {
            if (Math.Abs(rb.velocity.y) <= 0)
            {
                State = PlayerState.Falling;
            }
            else
            {
                State = PlayerState.Jumping;
            }
        }
    }

    void FixedUpdate()
    {
        float x = 0, y = rb.velocity.y;
        if (Input.GetButton("Horizontal"))
        {
            x = Input.GetAxis("Horizontal") * XSpeed * Time.fixedDeltaTime * 100;
        }

        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x - (x + px) / 80, transform.position.y), new Vector2(coll.bounds.size.x * 0.9f, 0.1f), 0, Vector2.down, Mathf.Infinity, 1);
        isGrounded = hit.collider != null && hit.distance < Grounding + coll.bounds.size.y / 2;

        if (wasGrounded && !isGrounded && rb.velocity.y >= 0)
        {
            coyote.Restart();
        }

        if (!Input.GetButton("Jump"))
        {
            jumpReleased = true;
        }

        if (jumpReleased || (startJump.IsRunning && startJump.Elapsed > varJumpTimeSpan))
        {
            rb.gravityScale = gravScale;
            startJump.Stop();
        }

        if ((isGrounded || (coyote.IsRunning && coyote.Elapsed < coyoteTimeSpan)) && jumpReleased && Input.GetButton("Jump"))
        {
            y = JumpHeight;
            jumpReleased = false;
            startJump.Restart();
            rb.gravityScale = VarJumpGravScale;
        }

        rb.velocity = new Vector2(x, y);
        px = x;
        wasGrounded = isGrounded;
    }
}
