using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NestedVisibilityHack : MonoBehaviour
{
  public VisibilityTrigger NestedVis;
  public VisibilityTrigger ParentVis;
  private DynamicText text;
  private int needsUpdate;
  // Start is called before the first frame update
  void Awake()
  {
    text = GetComponent<DynamicText>();
    text.VisibilityChanged += OnStateChange;
  }

  private void OnStateChange()
  {
    /* if (text.IsVisible) { */
    /*   if (ParentVis.Trigger != null && !ParentVis.Trigger.State && ParentVis.Invisible.SelectMany(x => x.GetComponentsInChildren<DynamicText>()).Contains(text)) { */
    /*     text.IsVisible = false; */
    /*   } */
    /*  else if (ParentVis.Trigger != null && ParentVis.Trigger.State && ParentVis.Visible.SelectMany(x => x.GetComponentsInChildren<DynamicText>()).Contains(text)) { */
    /*     text.IsVisible = false; */
    /*   } */
    /*   else if (NestedVis.Trigger != null && !NestedVis.Trigger.State && NestedVis.Invisible.SelectMany(x => x.GetComponentsInChildren<DynamicText>()).Contains(text)) { */
    /*     text.IsVisible = false; */
    /*   } */
    /*   else if (NestedVis.Trigger != null && NestedVis.Trigger.State && NestedVis.Visible.SelectMany(x => x.GetComponentsInChildren<DynamicText>()).Contains(text)) { */
    /*     text.IsVisible = false; */
    /*   } */
    /* } */
  }

  /* void LateUpdate() */
  /* { */
  /*   if (needsUpdate > 0) */
  /*   { */
  /*     needsUpdate--; */
  /*   } */
  /*   if (needsUpdate == 0) */
  /*   { */
  /*     needsUpdate = -1; */
  /*     if (ParentVis.Trigger.State) */
  /*     { */
  /*       NestedVis.StateChanged(); */
  /*     } */
  /*     else */
  /*     { */
  /*       if (NestedVis != null) */
  /*       { */
  /*         foreach (var go in NestedVis.Visible) */
  /*         { */
  /*           if (go.gameObject == this.gameObject) { continue; } */
  /*           go.SetIsVisibleQuiet(false); */
  /*         } */
  /*       } */
  /*     } */
  /*   } */
  /* } */
}
