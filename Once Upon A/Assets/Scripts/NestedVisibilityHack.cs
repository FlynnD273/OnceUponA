using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedVisibilityHack : MonoBehaviour
{
  public VisibilityTrigger NestedVis;
  public VisibilityTrigger ParentVis;
  private int needsUpdate;
  // Start is called before the first frame update
  void Awake()
  {
    if (ParentVis == null)
    {
      ParentVis = GetComponent<VisibilityTrigger>();
    }
    ParentVis.Trigger.StateChanged += OnStateChange;
  }

  private void OnStateChange()
  {
    needsUpdate = 3;
  }

  void LateUpdate()
  {
    if (needsUpdate > 0)
    {
      needsUpdate--;
    }
    if (needsUpdate == 0)
    {
      needsUpdate = -1;
      NestedVis.StateChanged();
    }
  }
}
