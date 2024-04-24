using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
  public bool Deactivated;
  public TriggerLogic Trigger;
  public DynamicText[] Visible;
  public DynamicText[] Invisible;

  public DangerController[] Dangerous;
  public DangerController[] Nondangerous;

  public void Start()
  {
    if (Trigger == null)
    {
      Trigger = GetComponent<TriggerLogic>();
    }
    Trigger.StateChanged += StateChanged;
  }

  public void StateChanged()
  {
    if (Deactivated)
    {
      return;
    }
    foreach (var go in Visible)
    {
      go.IsVisible = !Trigger.State;
    }
    foreach (var go in Invisible)
    {
      go.IsVisible = Trigger.State;
    }
    foreach (var go in Dangerous)
    {
      go.IsDangerous = !Trigger.State;
    }
    foreach (var go in Nondangerous)
    {
      go.IsDangerous = Trigger.State;
    }

    if (Trigger.State)
    {
      foreach (var go in Trigger.InvolvedSlots)
      {
        var slot = go.GetComponent<WordSlotController>();
        if (slot != null)
        {
          slot.IsSwappable = false;
        }
      }
      GameManager.Manager.JustActivated();
    }
  }
}
