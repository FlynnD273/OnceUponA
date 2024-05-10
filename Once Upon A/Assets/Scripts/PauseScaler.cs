using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Utils;

public class PauseScaler : MonoBehaviour
{
  public float StartSize = 1;
  public ExpDamp Scale;

  void Awake()
  {
    Scale = new(StartSize, 1, () => transform.localScale = Vector3.one * Scale.Value);
  }

  void Update()
  {
    Scale.Next(10, Time.unscaledDeltaTime);
  }
}
