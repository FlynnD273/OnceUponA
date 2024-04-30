using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Utils;

public class PauseScaler : MonoBehaviour
{
  private float scale;
  public float Scale
  {
    get => scale;
    set
    {
      scale = value;
      transform.localScale = Vector3.one * Scale;
    }
  }

  public float targetScale;


  void Update()
  {
    Scale = ExpDamp(Scale, targetScale, 10);
  }
}
