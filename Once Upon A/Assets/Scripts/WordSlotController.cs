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
        text.text = ""; //new string('_', (int)(trigger.bounds.size.x / 0.05 / 25));
        /* text.color = WordToColor[WordType.Normal]; */
      }
      else
      {
        text.color = WordToColor[value.Type];
        text.text = value.Text;
      }

      if (line != null)
      {
        line.startColor = WordToColor[value?.Type ?? WordType.White];
        line.endColor = line.startColor;
      }
    }
  }

  public WordType StartingWordType;
  public string TriggerWord;
  public bool Invert;
  public bool IsSwappable = true;

  private Collider2D trigger;
  private TextMesh text;

  private int startLength;
  private LineRenderer line;

  private AudioSource audioSource;

  private Word savedWord;

  // Start is called before the first frame update
  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    text = GetComponent<TextMesh>();
    startLength = text.text.Length;
    trigger = GetComponent<Collider2D>();

    if (IsSwappable)
    {
      line = gameObject.AddComponent<LineRenderer>();
      line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
      line.startColor = WordToColor[CurrentWord?.Type ?? WordType.White];
      line.startWidth = 0.15f;
      line.endColor = line.startColor;
      line.endWidth = line.startWidth;
      line.numCapVertices = 5;
      line.positionCount = 2;
      line.SetPositions(new Vector3[] { new Vector3(transform.position.x, transform.position.y - 1.25f), new Vector3(transform.position.x + trigger.bounds.size.x, transform.position.y - 1.25f) });
    }
  }

  public override void Init()
  {
    base.Init();
    if (text.text.Trim('_').Length == 0)
    {
      CurrentWord = null;
    }
    else
    {
      CurrentWord = new(StartingWordType, text.text);
    }

    GameManager.Manager.ResetOccurred += Reset;
    GameManager.Manager.SaveStateOccurred += SaveState;
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

  void OnDestroy() {
    GameManager.Manager.ResetOccurred -= Reset;
    GameManager.Manager.SaveStateOccurred -= SaveState;
  }
}
