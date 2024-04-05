using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CorruptAnimator : MonoBehaviour
{
  public float XIntensity;
  public float YIntensity;
  public float Probability;
  public float Length;

  private Vector2 startPos;
  private float startTime;
  private bool glitch;

  void Start()
  {
    startPos = transform.position;
    startTime = Time.time;
  }

  void Update()
  {
    if (Time.time - startTime > Length)
    {
      startTime = Time.time;
      glitch = Random.Range(0f, 1f) < Probability;
    }

    if (glitch)
    {
      float x = Random.Range(startPos.x - XIntensity, startPos.x + XIntensity);
      float y = Random.Range(startPos.y - YIntensity, startPos.y + YIntensity);
      transform.position = new Vector2(x, y);
    }
    else
    {
      transform.position = startPos;
    }
  }
}
