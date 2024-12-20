using UnityEngine;
using static Utils.Constants;

public class WordSlotController : TriggerLogic
{
    private Word currentWord;
    public Word CurrentWord
    {
        get => currentWord;
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

    private LineRenderer line;

    private readonly ExpDamp lineLength = new();

    private Word savedWord;
    private bool savedIsSwappable;

    public void Awake()
    {
        InvolvedSlots = new TriggerLogic[] { this };
        text = GetComponent<DynamicText>();
        trigger = GetComponent<BoxCollider2D>();

        line = gameObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        line.startWidth = 0.15f;
        line.endWidth = line.startWidth;
        line.SetPositions(new Vector3[] { new(0, -25), new(0, -25) });
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
        lineLength.TargetValue = text.Width;

        trigger.size = new Vector2(text.Width, trigger.size.y);
        trigger.offset = new Vector2(text.Width / 2, trigger.offset.y);
    }

    public override void Init()
    {
        base.Init();
        CurrentWord = text.Text.Trim('_').Length == 0 ? null : new(StartingWordType, text.Text);

        GameManager.Manager.ResetOccurred += Reset;
        GameManager.Manager.SaveStateOccurred += SaveState;
        IsSwappable = IsSwappableAtStart;
    }

    internal override void InheritedUpdate()
    {
        line.SetPosition(1, new Vector2(lineLength.Next(StandardAnim, Time.deltaTime), -25));
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
        Word temp = CurrentWord;
        CurrentWord = newWord;
        return temp;
    }

    public void OnDestroy()
    {
        GameManager.Manager.ResetOccurred -= Reset;
        GameManager.Manager.SaveStateOccurred -= SaveState;
    }
}
