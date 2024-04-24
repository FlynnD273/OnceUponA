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
      if (go == null) { continue; }
      go.IsVisible = !Trigger.State;
    }
    foreach (var go in Invisible)
    {
      if (go == null) { continue; }
      go.IsVisible = Trigger.State;
    }
    foreach (var go in Dangerous)
    {
      if (go == null) { continue; }
      go.IsDangerous = !Trigger.State;
    }
    foreach (var go in Nondangerous)
    {
      if (go == null) { continue; }
      go.IsDangerous = Trigger.State;
    }

    if (Trigger.State)
    {
      foreach (var go in Trigger.InvolvedSlots)
      {
        if (go == null) { continue; }
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
