using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Utils.Constants;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    public float XSpeed;
    public float Friction;
    public float JumpHeight;
    public float BouncyJumpHeight;
    public float VarJumpTime;
    public float VarJumpGravScale;
    public float CoyoteTime;
    public float Grounding;
    public HeldWordController HeldWordControl;

    public PlayerState State;
    public WordSlotController interactingSlot;

    private Stopwatch coyote;
    private Stopwatch startJump;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumping;
    private bool jumpReleased;
    private bool isBouncy;

    private TimeSpan varJumpTimeSpan;
    private TimeSpan coyoteTimeSpan;

    private float px;
    private float gravScale;

    private TimeSpan lockDuration;
    private Stopwatch lockControls;

    public enum PlayerState { Idle, Walking, Jumping, Falling }
    private Vector2 spawnPoint;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        gravScale = rb.gravityScale;
        coyote = new();
        startJump = new();
        lockControls = new();
        varJumpTimeSpan = new(0, 0, 0, 0, (int)(1000 * VarJumpTime));
        coyoteTimeSpan = new(0, 0, 0, 0, (int)(1000 * CoyoteTime));
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (Math.Abs(rb.velocity.x) <= 0.1f)
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

        if (Math.Abs(rb.velocity.x) >= 0.1f)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * (rb.velocity.x > 0 ? 1 : -1), transform.localScale.y);
        }

        anim.SetInteger("PlayerState", (int)State);

        if (Input.GetButtonDown("Reset"))
        {
            transform.position = spawnPoint;
            rb.velocity = new Vector2(0, 0);
            lockControls.Reset();
        }

        if (!lockControls.IsRunning)
        {
            if (isGrounded && interactingSlot != null && Input.GetButtonDown("Swap"))
            {
                HeldWordControl.HeldWord = interactingSlot.Swap(HeldWordControl.HeldWord);
                if ((interactingSlot.CurrentWord?.Type ?? WordType.Normal) == WordType.Bouncy)
                {
                    isBouncy = true;
                }
                else
                {
                    isBouncy = false;
                }
            }
        }
    }

    void FixedUpdate()
    {

        if (lockControls.IsRunning)
        {
            if (lockControls.Elapsed > lockDuration)
            {
                lockControls.Stop();
            }
            else
            {
                return;
            }
        }

        float x = 0, y = rb.velocity.y;
        if (Input.GetButton("Horizontal"))
        {
            x = Input.GetAxis("Horizontal") * XSpeed * Time.fixedDeltaTime * 100;
        }
        else
        {
            x = rb.velocity.x * Friction;
        }

        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x + coll.offset.x - (x + px) / 80, transform.position.y + coll.offset.y), new Vector2(coll.bounds.size.x * 0.9f, 0.1f), 0, Vector2.down, Mathf.Infinity, 1);
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
            if (isBouncy)
            {
                y = BouncyJumpHeight;
            }
            else
            {
                y = JumpHeight;
            }
            jumpReleased = false;
            isGrounded = false;
            startJump.Restart();
            rb.gravityScale = VarJumpGravScale;
        }

        rb.velocity = new Vector2(x, y);
        px = x;
        wasGrounded = isGrounded;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var slot = collision.gameObject.GetComponent<WordSlotController>();
        if (slot != null)
        {
            interactingSlot = slot;
            if ((slot.CurrentWord?.Type ?? WordType.Normal) == WordType.Bouncy)
            {
                isBouncy = true;
            }
            else
            {
                isBouncy = false;
            }
        }

        if (collision.gameObject.layer == (int)Layers.Checkpoint)
        {
            spawnPoint = transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var slot = collision.gameObject.GetComponent<WordSlotController>();
        if (slot != null)
        {
            interactingSlot = null;
            isBouncy = false;
        }
    }

    public void LockControls(float seconds)
    {
        lockDuration = new TimeSpan(0, 0, 0, 0, (int)(seconds * 1000));
        lockControls.Restart();
        rb.gravityScale = gravScale;
    }
}
