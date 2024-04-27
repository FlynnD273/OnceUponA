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
    if (Deactivated || Trigger == null)
    {
      return;
    }
    /* var text = GetComponent<DynamicText>(); */
    /* if (text != null && !text.IsVisible) */
    /* { */
    /*   foreach (var go in Visible) */
    /*   { */
    /*     go.SetVisibility(false, this); */
    /*   } */
    /*   foreach (var go in Invisible) */
    /*   { */
    /*     go.SetVisibility(false, this); */
    /*   } */
    /*   return; */
    /* } */
    foreach (var go in Visible)
    {
      go.SetVisibility(!Trigger.State, this);
    }
    foreach (var go in Invisible)
    {
      go.SetVisibility(Trigger.State, this);
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
      GameManager.Manager?.JustActivated();
    }
  }
}
