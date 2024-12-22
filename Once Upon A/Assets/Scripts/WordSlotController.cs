using System;
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

    private Word savedWord;
    private bool savedIsSwappable;
    private readonly Material textMaterial;

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
        WordType type = CurrentWord?.Type ?? WordType.White;
        if (!text.IsVisible || (type != WordType.Bouncy && type != WordType.Danger)) // || !IsSwappable)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;
        line.startColor = WordToColor[type];
        line.endColor = line.startColor;

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

    public new void Update()
    {
        base.Update();
        WordType type = CurrentWord?.Type ?? WordType.White;
        float len = text.Width;
        if (type == WordType.Danger)
        {
            const float spacing = 10f;
            line.positionCount = (int)(len / spacing) + 4;
            Vector3[] positions = new Vector3[line.positionCount];
            const float height = 4;
            int i;

            for (i = 0; i < positions.Length - 3; i++)
            {
                positions[i] = new Vector2(i * spacing, -27 + ((i % 2) == 0 ? height : -height));
            }

            float prevY = -27 + (((i - 1) % 2) == 0 ? height : -height);
            float y = -27 + ((i % 2) == 0 ? height : -height);
            float l = (len - ((i - 1) * spacing)) / spacing;
            positions[i] = new Vector2(len, Mathf.Lerp(prevY, y, l));

            i++;
            positions[i] = new Vector2(len, -27 + height);

            i++;
            positions[i] = new Vector2(0, -27 + height);

            line.SetPositions(positions);
            line.loop = true;
        }
        else if (type == WordType.Bouncy)
        {
            line.loop = false;
            line.positionCount = 2;
            line.SetPosition(0, new Vector2(0, -25));
            line.SetPosition(1, new Vector2(len, -25));
        }
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
