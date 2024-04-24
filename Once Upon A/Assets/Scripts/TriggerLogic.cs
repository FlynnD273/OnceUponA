using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
  public bool DebugState;
  public TriggerLogic[] InvolvedSlots = new TriggerLogic[0];
  private bool didInit;

  public event Action StateChanged;
  private bool state;
  public bool State
  {
    get
    {
      return state;
    }
    internal set
    {
      state = value;
      DebugState = state;
      StateChanged?.Invoke();
    }
  }

  public void Update()
  {
    if (GameManager.Manager.IsPaused) { return; }

    if (!didInit)
    {
      Init();
      didInit = true;
    }
  }

  public virtual void Init()
  {
    State = true;
    State = false;
  }
}
