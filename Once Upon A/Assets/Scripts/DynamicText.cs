using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
  public event Action TextChanged;
  private TextMesh textMesh;

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


  void Start()
  {
    textMesh = GetComponent<TextMesh>();
  }

  private bool hasInit = false;
  void Update()
  {
    if (!hasInit)
    {
      Text = textMesh.text;
      hasInit = true;
    }
  }

  public float Width
  {
    get
    {
      if (string.IsNullOrEmpty(Text))
      {
        return GameManager.Manager.fontAdvances['M'] * 3;
      }
      float sum = 0;
      foreach (var c in Text)
      {
        if (GameManager.Manager.fontAdvances.TryGetValue(c, out float w))
        {
          sum += w;
        }
        else if (!char.IsWhiteSpace(c))
        {
          Debug.LogWarning($"Unknown character \'{Regex.Escape("" + c)}\'");
        }
      }
      return sum;
    }
  }
}
