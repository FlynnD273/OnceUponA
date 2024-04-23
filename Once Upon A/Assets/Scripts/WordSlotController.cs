using static Utils.Constants;
using UnityEngine;

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
        text.Text = "";
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
      isSwappable = value;
      UpdateUnderline();
      text.Text = text.Text;
    }
  }

  private BoxCollider2D trigger;
  private DynamicText text;

  private int startLength;
  private LineRenderer line;

  private AudioSource audioSource;

  private Word savedWord;
  private bool savedIsSwappable;

  // Start is called before the first frame update
  void Awake()
  {
    InvolvedSlots = new TriggerLogic[] { this };
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
    text.VisibilityChanged += UpdateUnderline;
  }

  public void UpdateUnderline()
  {
    if (!text.IsVisible || !IsSwappable)
    {
      line.enabled = false;
      return;
    }

    line.enabled = true;
    line.startColor = WordToColor[CurrentWord?.Type ?? WordType.White];
    line.endColor = line.startColor;
    line.SetPositions(new Vector3[] { new Vector3(0, -25), new Vector3(text.Width, -25) });

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
    IsSwappable = IsSwappableAtStart;
  }

  private void Reset()
  {
    IsSwappable = savedIsSwappable;
    CurrentWord = savedWord;
  }

  private void SaveState()
  {
    savedWord = CurrentWord;
    savedIsSwappable = IsSwappable;
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
