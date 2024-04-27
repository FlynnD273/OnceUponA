using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
  public bool Deactivated;
  public TriggerLogic Trigger;
  public DynamicText[] Visible;
  public DynamicText[] Invisible;

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
    var text = GetComponent<DynamicText>();
    if (Deactivated || Trigger == null)
    {
      return;
    }
    if (text != null && !text.IsVisible)
    {
      foreach (var go in Visible)
      {
        go.IsVisible = false;
      }
      foreach (var go in Invisible)
      {
        go.IsVisible = false;
      }
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
