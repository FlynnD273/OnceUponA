using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private BoxCollider2D trigger;

    public TriggerLogic DeactivateTrigger;

    void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
        textMesh = GetComponent<TextMeshPro>();
        Position = new(transform.position, transform.position, () => transform.position = Position.Value);
    }

    void Start()
    {
        isDangerous = true;
        if (DeactivateTrigger != null)
        {
            DeactivateTrigger.StateChanged += () => IsDangerous = !DeactivateTrigger.State;
        }
        trigger.offset = new Vector2(Width / 2, trigger.offset.y);
        trigger.size = new Vector2(Width, trigger.size.y);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        bool triggered = (DeactivateTrigger != null && !DeactivateTrigger.State);
        if (IsVisible && (triggered || coll.gameObject.layer == (int)Layers.Player))
        {
            coll.GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.normalized.x * Power, Direction.normalized.y * Power);
            coll.GetComponent<PlayerController>().LockControls(LockTime);
        }
    }
}
