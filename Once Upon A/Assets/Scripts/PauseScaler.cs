using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Utils;

public class PauseScaler : MonoBehaviour
{
  public ExpDamp Scale;

  void Awake()
  {
    Scale = new(1, 1, () => transform.localScale = Vector3.one * Scale.Value);
  }

  void Update()
  {
    Scale.Next(10, Time.unscaledDeltaTime);
  }
}
