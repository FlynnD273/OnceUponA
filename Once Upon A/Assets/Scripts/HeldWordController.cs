using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;
using static Utils.Utils;

public class HeldWordController : MonoBehaviour
{
  public GameObject Target;
  public float Speed = 0.1f;
  private Vector3 offset = new(0, 1.8f, 0);
  private ExpDampVec3 position;

  private TextMesh heldWordMesh;
  private Word heldWord;
  public Word HeldWord
  {
    get
    {
      return heldWord;
    }
    set
    {
      heldWord = value;
      if (value == null)
      {
        heldWordMesh.text = "";
      }
      else
      {
        heldWordMesh.text = value.Text;
        heldWordMesh.color = WordToColor[value.Type];
        var xOffset = 1f * Mathf.Sign(Target.transform.localScale.x);
        position.Value = Target.transform.position + new Vector3(xOffset, -1);
      }
    }
  }

  private Word savedWord;

  void Awake()
  {
    heldWordMesh = GetComponent<TextMesh>();
    position = new(Vector3.zero, Vector3.zero, () => transform.position = position.Value);
  }

  void Start()
  {
    position.TargetValue = Target.transform.position + offset;
    position.Value = Target.transform.position + offset;
    GameManager.Manager.ResetOccurred += Reset;
    GameManager.Manager.SaveStateOccurred += SaveState;
  }

  private void Reset()
  {
    HeldWord = savedWord;
  }

  private void SaveState()
  {
    savedWord = HeldWord;
  }

  void FixedUpdate()
  {
    position.TargetValue = Target.transform.position + offset;
    position.Next(Speed, Time.fixedDeltaTime);
  }
}
