using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Utils.Constants;

public class DangerController : DynamicText
{
  private bool isDangerous;
  public bool IsDangerous
  {
    get => isDangerous;
    set
    {
      isDangerous = value;
      if (IsDangerous)
      {
        trigger.enabled = true;
        textMesh.color = Constants.WordToColor[WordType.Danger];
      }
      else
      {
        trigger.enabled = false;
        textMesh.color = Constants.WordToColor[WordType.White];
      }
    }
  }

  public Vector2 Direction;
  public float Power;
  public float LockTime = 0.5f;
  private Collider2D trigger;

  public TriggerLogic DeactivateTrigger;

  void Start()
  {
    isDangerous = true;
    trigger = GetComponent<Collider2D>(); 
  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.layer == (int)Layers.Player)
    {
      coll.GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.normalized.x * Power, Direction.normalized.y * Power);
      coll.GetComponent<PlayerController>().LockControls(LockTime);
    }
  }
}
