using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static Utils.Constants;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

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
  public List<WordSlotController> interactingSlots = new();

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
  private Vector3 spawnPoint;

  private Animator anim;

  private bool didSave;

  void Awake()
  {
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

  void Start()
  {
    GameManager.Manager.ResetOccurred += Reset;
    GameManager.Manager.SaveStateOccurred += SaveState;
  }

  private void SaveState()
  {
    spawnPoint = transform.position;
  }

  private void Reset()
  {
    transform.position = spawnPoint;
    rb.velocity = new Vector2(0, 0);
    lockControls.Reset();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Manager.IsPaused) { return; }

    if (!didSave)
    {
      GameManager.Manager.SaveState();
      didSave = true;
    }
    if (transform.position.y < -150)
    {
      GameManager.Manager.Reset();
    }
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
      if (rb.velocity.y <= 0)
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
      transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(rb.velocity.x), transform.localScale.y);
    }

    anim.SetInteger("PlayerState", (int)State);

    if (!lockControls.IsRunning)
    {
      var slot = GetCurrentSlot();
      if (isGrounded && slot != null && Input.GetButtonDown("Swap"))
      {
        HeldWordControl.HeldWord = slot.Swap(HeldWordControl.HeldWord);
        GameManager.Manager.JustSwapped();

        if ((slot.CurrentWord?.Type ?? WordType.Normal) == WordType.Bouncy)
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
      x *= Friction;
    }

    Physics2D.queriesHitTriggers = false;
    RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x + coll.offset.x - (x + px) / 80, transform.position.y + coll.offset.y), new Vector2(coll.bounds.size.x * 0.9f, 0.1f), 0, Vector2.down, Mathf.Infinity, 1);
    isGrounded = hit.collider != null && hit.distance < Grounding + coll.bounds.size.y / 2;

    if (wasGrounded && !isGrounded)
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
    /* transform.position += new Vector3(x * Time.fixedDeltaTime, 0); */
    px = x;
    wasGrounded = isGrounded;
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    var slot = collision.gameObject.GetComponent<WordSlotController>();
    if (slot != null)
    {
      interactingSlots.Add(slot);
      UpdateBouncy();
    }

    if (collision.gameObject.layer == (int)Layers.Checkpoint)
    {
      GameManager.Manager.SaveState();
    }
  }

  private void UpdateBouncy()
  {
    if (interactingSlots.Any(x => (x.CurrentWord?.Type ?? WordType.Normal) == WordType.Bouncy))
    {
      isBouncy = true;
    }
    else
    {
      isBouncy = false;
    }
  }

  void OnTriggerExit2D(Collider2D collision)
  {
    var slot = collision.gameObject.GetComponent<WordSlotController>();
    if (slot != null)
    {
      interactingSlots.Remove(slot);
      UpdateBouncy();
    }
  }

  public void LockControls(float seconds)
  {
    lockDuration = new TimeSpan(0, 0, 0, 0, (int)(seconds * 1000));
    lockControls.Restart();
    rb.gravityScale = gravScale;
  }

  public WordSlotController GetCurrentSlot()
  {
    return interactingSlots.FirstOrDefault(x => x.IsSwappable);
  }

  void OnDestroy()
  {
    GameManager.Manager.ResetOccurred -= Reset;
    GameManager.Manager.SaveStateOccurred -= SaveState;
  }
}
