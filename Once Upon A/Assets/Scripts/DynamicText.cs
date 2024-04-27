using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Utils;

public class DynamicText : MonoBehaviour
{
  public Vector3 TargetPosition;
  public event Action TextChanged;
  public event Action VisibilityChanged;
  internal TextMesh textMesh;
  private bool savedIsVisible;
  private bool isVisible;
  public bool IsVisible
  {
    get => isVisible;
    set
    {
      if (isVisible != value)
      {
        isVisible = value;
        UpdateVisibility();
      }
    }
  }

  public string Text
  {
    get
    {
      if (textMesh != null)
      {
        return textMesh.text;
      }
      return "";
    }
    set
    {
      if (textMesh != null)
      {
        textMesh.text = value;
      }
      TextChanged?.Invoke();
    }
  }

  public Color Color
  {
    get => textMesh?.color ?? Color.white;
    set
    {
      if (textMesh != null)
      {
        textMesh.color = value;
      }
      TextChanged?.Invoke();
    }
  }

  void Awake() // DON'T FORGET TO COPY TO DANGER CONTROLLER
  {
    textMesh = GetComponent<TextMesh>();
    TargetPosition = transform.position;
    IsVisible = true;
  }

  void FixedUpdate()
  {
    if (GameManager.Manager.IsPaused) { return; }

    if ((transform.position - TargetPosition).sqrMagnitude < 0.1f)
    {
      transform.localPosition = TargetPosition;
    }
    else
    {
      transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, 0.5f);
    }
  }

  public float Width
  {
    get
    {
      var spacingCont = GetComponent<TextSpacingController>();
      if (spacingCont != null)
      {
        return spacingCont.Width;
      }
      var slot = GetComponent<WordSlotController>();
      if (!IsVisible || (slot != null && !slot.IsSwappable && slot.CurrentWord == null))
      {
        return 0;
      }

      if (Text == "")
      {
        return Constants.CharWidths['M'] * 3;
      }
      float sum = 0;
      foreach (var c in Text)
      {
        sum += Constants.CharWidths[c];
      }
      return sum;
    }
  }

  private bool init = false;
  void Update()
  {
    if (!init)
    {
      init = true;
      GameManager.Manager.ResetOccurred += Reset;
      GameManager.Manager.SaveStateOccurred += SaveState;
    }
  }

  private void UpdateVisibility()
  {
    GetComponent<Renderer>().enabled = isVisible;
    foreach (var coll in GetComponents<Collider2D>())
    {
      coll.enabled = isVisible;
    }
    foreach (var child in GetComponentsInChildren<DynamicText>())
    {
      if (child.gameObject == this.gameObject)
      {
        continue;
      }
      child.IsVisible = isVisible;
    }
    var vis = GetComponent<VisibilityTrigger>();
    if (IsVisible && vis != null)
    {
      vis.StateChanged();
    }
    VisibilityChanged?.Invoke();
  }

  private void Reset()
  {
    /* IsVisible = savedIsVisible; */
  }

  private void SaveState()
  {
    /* savedIsVisible = IsVisible; */
  }

  void OnDestroy()
  {
    GameManager.Manager.ResetOccurred -= Reset;
    GameManager.Manager.SaveStateOccurred -= SaveState;
  }

  public void SetIsVisibleQuiet(bool isVisible)
  {
    this.isVisible = isVisible;
    GetComponent<Renderer>().enabled = isVisible;
    foreach (var coll in GetComponents<Collider2D>())
    {
      coll.enabled = isVisible;
    }
    foreach (var child in GetComponentsInChildren<DynamicText>())
    {
      if (child.gameObject == this.gameObject)
      {
        continue;
      }
      child.IsVisible = isVisible;
    }
  }
}
