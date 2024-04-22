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
  internal TextMesh textMesh;

  public string Text
  {
    get => textMesh.text;
    set
    {
      textMesh.text = value;
      TextChanged?.Invoke();
    }
  }

  public Color Color
  {
    get => textMesh.color;
    set
    {
      textMesh.color = value;
      TextChanged?.Invoke();
    }
  }


  void Awake()
  {
    textMesh = GetComponent<TextMesh>();
    TargetPosition = transform.position;
  }

  void FixedUpdate()
  {
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
      var slot = GetComponent<WordSlotController>();
      if (slot != null && !slot.IsSwappable && slot.CurrentWord == null)
      {
        return -Constants.CharWidths[' '];
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

  public void SetVisibility(bool isVisible)
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
      child.SetVisibility(isVisible);
    }

    var slot = GetComponent<WordSlotController>();
    if (slot != null)
    {
      slot.UpdateUnderline();
    }
  }
}
