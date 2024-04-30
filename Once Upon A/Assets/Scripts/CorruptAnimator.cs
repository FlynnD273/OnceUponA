using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Utils;
using static Utils.Constants;
using static Utils.Utils;

public class CorruptAnimator : DynamicText
{
  public string UncorruptWord;
  public float XIntensity;
  public float YIntensity;
  public float Probability;
  public float Length;
  public TriggerLogic Trigger;

  private string corruptWord;
  private Vector3 lerpedPos;
  private float startTime;
  private bool glitch;

  void Start()
  {
    lerpedPos = transform.localPosition;
    startTime = Time.time;
    corruptWord = textMesh.text;

    if (Trigger == null)
    {
      Trigger = GetComponent<TriggerLogic>();
    }
    Trigger.StateChanged += Switch;
  }

  void FixedUpdate()
  {
    if (Trigger.State)
    {
      glitch = false;
    }
    else if (Time.time - startTime > Length)
    {
      startTime = Time.time;
      glitch = Random.Range(0f, 1f) < Probability;
    }

    Vector3 offset = Vector3.zero;
    if (glitch)
    {
      float x = Random.Range(-XIntensity, XIntensity);
      float y = Random.Range(-YIntensity, YIntensity);
      offset = new Vector3(x, y, 0) / 0.05f * transform.localScale.x;
    }

    lerpedPos = Anim(lerpedPos, TargetPosition, Constants.StandardAnim);

    transform.localPosition = lerpedPos + offset;
  }

  private void Switch()
  {
    if (Trigger.State)
    {
      textMesh.color = WordToColor[WordType.White];
      Text = UncorruptWord;
    }
    else
    {
      textMesh.color = WordToColor[WordType.Corrupt];
      Text = corruptWord;
    }
  }
}
