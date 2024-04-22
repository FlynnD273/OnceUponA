using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
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
    foreach (var go in Visible)
    {
      if (go == null) { continue; }
      go.SetVisibility(!Trigger.State);
    }
    foreach (var go in Invisible)
    {
      if (go == null) { continue; }
      go.SetVisibility(Trigger.State);
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
