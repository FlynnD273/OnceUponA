using System.Collections;
using System.Collections.Generic;
using static Utils.Constants;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using System.Linq;

public class WordSlotController : TriggerLogic
{
  private Word currentWord;
  public Word CurrentWord
  {
    get
    {
      return currentWord;
    }
    set
    {
      currentWord = value;
      bool cond;
      if (string.IsNullOrEmpty(TriggerWord))
      {
        cond = !string.IsNullOrWhiteSpace(CurrentWord?.Text);
      }
      else
      {
        cond = CurrentWord?.Text == TriggerWord;
      }
      cond = Invert ? !cond : cond;
      State = cond;

      if (value == null)
      {
        text.Text = ""; //new string('_', (int)(trigger.bounds.size.x / 0.05 / 25));
        /* text.color = WordToColor[WordType.Normal]; */
      }
      else
      {
        text.Color = WordToColor[value.Type];
        text.Text = value.Text;
      }

      line.startColor = WordToColor[value?.Type ?? WordType.White];
      line.endColor = line.startColor;
    }
  }

  public WordType StartingWordType;
  public string TriggerWord;
  public bool Invert;
  public bool IsSwappableAtStart = true;


  private bool isSwappable = true;
  public bool IsSwappable
  {
    get => isSwappable;
    set
    {
      UpdateUnderline();
      isSwappable = value;
    }
  }

  private BoxCollider2D trigger;
  private DynamicText text;

  private int startLength;
  private LineRenderer line;

  private AudioSource audioSource;

  private Word savedWord;

  // Start is called before the first frame update
  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    text = GetComponent<DynamicText>();
    trigger = GetComponent<BoxCollider2D>();

    line = gameObject.AddComponent<LineRenderer>();
    line.useWorldSpace = false;
    line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    line.startWidth = 0.15f;
    line.endWidth = line.startWidth;
    line.numCapVertices = 5;
    line.positionCount = 2;

    text.TextChanged += UpdateUnderline;
  }

  private void UpdateUnderline()
  {
    if (!IsSwappable)
    {
      line.enabled = false;
      return;
    }
    else
    {
      line.enabled = true;
    }
    line.startColor = WordToColor[CurrentWord?.Type ?? WordType.White];
    line.endColor = line.startColor;
    line.SetPositions(new Vector3[] { new Vector3(0, -1.25f / transform.localScale.y), new Vector3(text.Width, -1.25f / transform.localScale.y) });

    trigger.size = new Vector2(text.Width, trigger.size.y);
    trigger.offset = new Vector2(text.Width / 2, trigger.offset.y);
  }

  public override void Init()
  {
    base.Init();
    if (text.Text.Trim('_').Length == 0)
    {
      CurrentWord = null;
    }
    else
    {
      CurrentWord = new(StartingWordType, text.Text);
    }

    GameManager.Manager.ResetOccurred += Reset;
    GameManager.Manager.SaveStateOccurred += SaveState;
    UpdateUnderline();
  }

  private void Reset()
  {
    CurrentWord = savedWord;
  }

  private void SaveState()
  {
    savedWord = CurrentWord;
  }

  public Word Swap(Word newWord)
  {
    var temp = CurrentWord;
    CurrentWord = newWord;
    return temp;
  }

  void OnDestroy()
  {
    GameManager.Manager.ResetOccurred -= Reset;
    GameManager.Manager.SaveStateOccurred -= SaveState;
  }
}
